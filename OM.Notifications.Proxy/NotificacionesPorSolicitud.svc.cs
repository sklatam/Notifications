using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OM.Common.Utils.WcfClient;
using OM.Notifications.Facade.Services;
using NLog;
using Newtonsoft.Json;
using System.Data;
using System.Linq;
using SkCo.Notificaciones.DSL;
using OM.Notifications.Entities;
using OM.Notifications.Facade.Mapping;

namespace OM.Notifications.Facade
{
    /// <summary>
    /// Esta clase es una fachada para el servicio Legacy SkCo (NotificacionesPorSolicitud),
    /// el cual es consumido por Mexico y las aplicaciones de Colombia que no hayan sido migradas.
    /// 
    /// Según la configuracion de uso del SkCoLegacy (Settings) utiliza el servicio legacy (BizTalk) o el nuevo (Windows Service)
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class NotificacionesPorSolicitud : INotificacionesPorSolicitud
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public void EnqueueMessage(int idCategoria, string tipoIdCliente, string numIdCliente, string contrato, string monto, string NroConfirmacion, string IP, string Descripcion, string producto)
        {
            try
            {
                if (Properties.Settings.Default.UseProxyForContacts)
                {
                    this._logger.Info("NotificacionesPorSolicitud.Proxy > EnqueueMessage");

                    new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().Invoke(
                        c => c.MSMQMessage(idCategoria, tipoIdCliente, numIdCliente, contrato, monto, producto, null));
                }
                else
                {
                    var contract = string.IsNullOrEmpty(contrato) ? 0 : Convert.ToInt32(contrato);
                    var amount = string.IsNullOrEmpty(monto) ? 0 : Convert.ToInt32(monto);
                    
                    Services.Factory.Build()
                        .SendMessage(idCategoria, numIdCliente, tipoIdCliente, contract, producto, amount, null, null);
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                throw;
            }
        }

        public void MSMQMessage(int idCategoria, string tipoIdCliente, string numIdCliente, string contrato, string monto, string producto, DSNotificaciones MensajesAdicionales)
        {
            try
            {
                if (Properties.Settings.Default.UseProxyForContacts)
                {
                    this._logger.Info("NotificacionesPorSolicitud.Proxy > MSMQMessage");

                    new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().Invoke(
                        c => c.MSMQMessage(idCategoria, tipoIdCliente, numIdCliente, contrato, monto, producto, MensajesAdicionales));
                }
                else
                {
                    var contract = string.IsNullOrEmpty(contrato) ? 0 : Convert.ToInt32(contrato);
                    var amount = string.IsNullOrEmpty(monto) ? 0 : Convert.ToInt32(monto);

                    var variables = this.GetMessageVariable(MensajesAdicionales);

                    Services.Factory.Build(Properties.Settings.Default.UseProxyForMSMQMessage)
                        .SendMessage(idCategoria, numIdCliente, tipoIdCliente, contract, producto, amount, variables, null);
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                throw;
            }
        }

        public string MSMQMessage_Mexico(int Operacion, string tipoIdCliente, string numIdCliente, string contrato, decimal monto, string mobilCliente, string mobilFP, string emailCliente, string emailFP, DSNotificaciones MensajesAdicionales, out string detallError)
        {
            //Inicializar variable detalle error en blanco
            detallError = string.Empty;

            try
            {
                if (Properties.Settings.Default.UseProxyForContacts)
                {
                    this._logger.Info("NotificacionesPorSolicitud.Proxy > MSMQMessage_Mexico");

                    return new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient()
                        .MSMQMessage_Mexico(out detallError, Operacion, tipoIdCliente, numIdCliente, contrato, monto, mobilCliente, mobilFP, emailCliente, emailFP, MensajesAdicionales);
                }
                else
                {
                    var rulesHelper = new Processors.DataHelpers.MessageRuleHelper();

                    #region Validar parametros

                    if (!rulesHelper.GetOperation(Operacion))
                        detallError += "- La operación enviada no existe.\n";

                    if (string.IsNullOrEmpty(tipoIdCliente))
                        detallError += "- El tipo de identificación no es valido (blanco/nulo).\n";
                    else
                        tipoIdCliente = tipoIdCliente.Trim();

                    if (string.IsNullOrEmpty(numIdCliente))
                        detallError += "- El número de identificación no es valido (blanco/nulo).\n";
                    else
                        numIdCliente = numIdCliente.Trim();

                    if (string.IsNullOrEmpty(contrato))
                        detallError += "- El contrato no es valido (blanco/nulo).\n";
                    else
                        contrato = contrato.Trim();

                    #endregion

                    if (detallError != string.Empty)
                        return "ERROR_PAR";

                    var variables = this.GetMessageVariable(MensajesAdicionales);
                    var contacts = this.GetContacts(mobilCliente, emailCliente, mobilFP, emailFP, contrato);

                    Services.Factory.Build(Properties.Settings.Default.UseProxyForMSMQMessage_Mexico)
                        .SendMessageWithContacts(Operacion, numIdCliente, tipoIdCliente, Convert.ToInt32(contrato ?? "0"), monto, variables, contacts);
                }
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Proxy > MSMQMessage_Mexico: {ex.Message}");
                detallError = ex.Message;
                return "ERROR_INT";
            }

            return "OK";
        }

        private Entities.MessageVariable[] GetMessageVariable(DSNotificaciones messageVariable)
        {
            // Se realiza convert de variables de DataSet a un Array que espera el metodo de envio de mensaje
            if (messageVariable != null && messageVariable.Tables["EnvioMasivoMensajesAdicionales"] != null)
            {
                return messageVariable.Tables["EnvioMasivoMensajesAdicionales"]
                    .AsEnumerable()
                    .Select(
                        dataRow => new MessageVariable
                        {
                            Name = dataRow.Field<string>("Name"),
                            Value = dataRow.Field<string>("Value")
                        })
                    .ToArray();
            }

            return Enumerable.Empty<MessageVariable>().ToArray();
        }

        private Entities.MessageContact[] GetContacts(string movilCliente, string emailCliente, string movilFp, string emailFp, string contrato)
        {
            var contacts = new List<MessageContact>();

            if (!string.IsNullOrEmpty(movilCliente))
                contacts.Add(new MessageContact { Contact = movilCliente, Type = MessageType.SMS, Recipient = ContactRecipient.Customer, UnifiedReference = contrato });

            if (!string.IsNullOrEmpty(emailCliente))
                contacts.Add(new MessageContact { Contact = emailCliente, Type = MessageType.Email, Recipient = ContactRecipient.Customer, UnifiedReference = contrato });

            if (!string.IsNullOrEmpty(movilFp))
                contacts.Add(new MessageContact { Contact = movilFp, Type = MessageType.SMS, Recipient = ContactRecipient.Agent, UnifiedReference = contrato });

            if (!string.IsNullOrEmpty(emailFp))
                contacts.Add(new MessageContact { Contact = emailFp, Type = MessageType.Email, Recipient = ContactRecipient.Agent, UnifiedReference = contrato });

            return contacts.ToArray();
        }

        // <<<<<<<< Metodos antiguos que se reporatron en  gemini ADM01-199718 >>>>>>>>
        public void C_E_comunicacion_Automatica_insert(string Campana, string Documento, string Tipo_Doc, string email, string campo1_text)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > C_E_comunicacion_Automatica_insert");
                //new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().C_E_comunicacion_Automatica_insert(Campana, Documento, Tipo_Doc, email, campo1_text);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > C_E_comunicacion_Automatica_insert > Datos entrada: {JsonConvert.SerializeObject(new object[] { Campana, Documento, Tipo_Doc, email, campo1_text })}");
                new Data.AdministracionNotificacionesMexico.ComunicacionAutomaticaInsert().Run(new object[] { Campana, Documento, Tipo_Doc, email, campo1_text });
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > C_E_comunicacion_Automatica_insert (error) : {ex.Message}");
                throw;
            }
        }

        public System.Data.DataSet ConsultarMensajes_PreferenciasTransaccion(string Documento, string Tipo_Documento, int Pais)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > ConsultarMensajes_PreferenciasTransaccion");
                //return new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().ConsultarMensajes_PreferenciasTransaccion(Documento, Tipo_Documento, Pais);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > ConsultarMensajes_PreferenciasTransaccion > Datos entrada: {JsonConvert.SerializeObject(new object[] { Documento, Tipo_Documento, Pais })}");
                DataSet retorno = new Data.AdministracionNotificacionesMexico.ConsultarMensajesByPreferenciasTransaccion().Get(Documento, Tipo_Documento, Pais);
                retorno.Tables[0].TableName = "Editor";
                return retorno;
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > ConsultarMensajes_PreferenciasTransaccion (error) : {ex.Message}");
                throw;
            }
        }

        public System.Data.DataSet ConsultarPreferencias(int Pais)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > ConsultarPreferencias");
                //return new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().ConsultarPreferencias(Pais);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > ConsultarPreferencias > Datos entrada: {JsonConvert.SerializeObject(new object[] { Pais })}");
                DataSet retorno = new Data.AdministracionNotificacionesMexico.ConsultarPreferencias().Get(Pais);
                retorno.Tables[0].TableName = "Table1";
                return retorno;
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > ConsultarPreferencias (error) : {ex.Message}");
                throw;
            }
        }

        public string CrearPreferenciasTyCMx(int Pais, string user, string ClienteRFC, string Movil, string contrato, string tipoTerminos)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > CrearPreferenciasTyCMx");
                //return new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().CrearPreferenciasTyCMx(Pais, User, RfcCliente, Movil, Contrato, TipoTerminos);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > CrearPreferenciasTyCMx > Datos entrada: {JsonConvert.SerializeObject(new object[] { Pais, user, ClienteRFC, Movil, contrato, tipoTerminos, null, 2 })}");
                return new Data.AdministracionNotificacionesMexico.CrearPreferenciasTyCMx().Get(Pais, user, ClienteRFC, Movil, contrato, tipoTerminos, null,2);
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > CrearPreferenciasTyCMx (error) : {ex.Message}");
                throw;
            }
        }

        public void EnqueueEmail_Mexico(int IdTabla, string Value)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > EnqueueEmail_Mexico");
                //new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().EnqueueEmail_Mexico(IdTabla, Value);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > EnqueueEmail_Mexico > Datos entrada: {JsonConvert.SerializeObject(new object[] { IdTabla, Value })}");
                new Data.AdministracionNotificacionesMexico.EnqueueEmailMexicoInsert().Run(new object[] { IdTabla , Value });
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > EnqueueEmail_Mexico (error) : {ex.Message}");
                throw;
            }
        }

        public void Notificacion_SMS_CreacionCuentas(string Producto, string Plan, string contrato)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > Notificacion_SMS_CreacionCuentas");
                //new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().Notificacion_SMS_CreacionCuentas(Producto, Plan, Contrato);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > Notificacion_SMS_CreacionCuentas > Datos entrada: {JsonConvert.SerializeObject(new object[] { Producto, "0", -1, Plan, "", contrato, Convert.ToDecimal(0), DateTime.Today, true, DateTime.Today })}");
                new Data.AdministracionNotificacionesMexico.NotificacionSmsCreacionCuentas().Run(new object[] { Producto, "0", -1, Plan, "", contrato, Convert.ToDecimal(0), DateTime.Today, true, DateTime.Today });
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > Notificacion_SMS_CreacionCuentas (error) : {ex.Message}");
                throw;
            }
        }

        public string NotificarTyCMx(int Pais, string user, string ClienteRFC, string Movil, string contrato, int tipoOperacion)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > NotificarTyCMx");
                //return new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().NotificarTyCMx(Pais, User, RfcCliente,Movil,Contrato,TipoOperacion);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > NotificarTyCMx > Datos entrada: {JsonConvert.SerializeObject(new object[] { Pais, user, ClienteRFC, Movil, contrato, null, tipoOperacion, 3 })}");
                return new Data.AdministracionNotificacionesMexico.NotificarTyCMx().Get(Pais,user, ClienteRFC, Movil,contrato,null, tipoOperacion, 3);
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > NotificarTyCMx (error) : {ex.Message}");
                throw;
            }
        }

        public string PreferenciasEditor(string PERS_IdNumber, string PERS_IdType, int IdMensaje, string UsuarioCrea, string UsuarioModifica, bool Sms, bool Email, string categoria, string contrato, string pais, string strRespuesta, string Movil)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > PreferenciasEditor");
                //return new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().PreferenciasEditor(PERS_IdNumber, PERS_IdType, IdMensaje, UsuarioCrea, UsuarioModifica, Sms,Email,Categoria,Contrato,Pais,StrRespuesta,Movil);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > PreferenciasEditor > Datos entrada: {JsonConvert.SerializeObject(new object[] { PERS_IdNumber, PERS_IdType, IdMensaje, UsuarioCrea, UsuarioModifica, Sms, Email, categoria, contrato, pais, strRespuesta, Movil })}");
                return new Data.AdministracionNotificacionesMexico.PreferenciasEditor().Get(PERS_IdNumber,PERS_IdType,IdMensaje,UsuarioCrea,UsuarioModifica,Sms,Email, categoria, contrato, pais, strRespuesta, Movil);
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > PreferenciasEditor (error) : {ex.Message}");
                throw;
            }
        }

        public string PreferenciasEditorEditar(string PERS_IdNumber, bool Sms, bool Email, string categoria)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > PreferenciasEditorEditar");
                //return new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().PreferenciasEditorEditar(PERS_IdNumber, Sms, Email, Categoria);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > PreferenciasEditorEditar > Datos entrada: {JsonConvert.SerializeObject(new object[] { PERS_IdNumber, Sms, Email, categoria })}");
                new Data.AdministracionNotificacionesMexico.PreferenciasEditorEditarRun().Run(PERS_IdNumber, Sms, Email, categoria);
                return new Data.AdministracionNotificacionesMexico.PreferenciasEditorEditarGet().Get( PERS_IdNumber, Sms , Email, categoria);
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > PreferenciasEditorEditar (error) : {ex.Message}");
                throw;
            }
        }


        public bool VerificarPreferenciasTyCMx(int Pais, string user, string ClienteRFC, string Movil, string contrato, string tipoTerminos)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > VerificarPreferenciasTyCMx");
                //return new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().VerificarPreferenciasTyCMx(Pais, User, RfcCliente, Movil, Contrato, TipoTerminos);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > VerificarPreferenciasTyCMx > Datos entrada: {JsonConvert.SerializeObject(new object[] { Pais, user, ClienteRFC, Movil, contrato, tipoTerminos, null, 1 })}");
                var retorno = new Data.AdministracionNotificacionesMexico.VerificarPreferenciasTyCMx().Get(Pais,user, ClienteRFC, Movil,contrato, tipoTerminos, null,1);
                return retorno > 0 ;
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > VerificarPreferenciasTyCMx (error) : {ex.Message}");
                throw;
            }
        }

        public void UpdatePreferenciasPortal(int operacion, string tipoidCliente, string idCliente, bool sms, bool mail, string user)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > UpdatePreferenciasPortal");
                //new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().UpdatePreferenciasPortal(Operacion, TipoidCliente, IdCliente, Sms, Mail, User);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > UpdatePreferenciasPortal > Datos entrada: {JsonConvert.SerializeObject(new object[] { operacion, tipoidCliente, idCliente, sms, mail, user })}");
                new Data.AdministracionNotificacionesMexico.UpdatePreferenciasPortal().Run(new object[] { operacion, tipoidCliente, idCliente, sms, mail, user });
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > UpdatePreferenciasPortal (error) : {ex.Message}");
                throw;
            }
        }

        public void UpdatePreferenciasPipeline(int operacion, string tipoidCliente, string idCliente, bool sms, bool mail, string user, bool NoSeleccionoRecibirViaSms, bool NoSeleccionoRecibirViaEmail)
        {
            try
            {
                this._logger.Info("NotificacionesPorSolicitud.Administracion_Mexico > UpdatePreferenciasPipeline");
                //new WsNotificacionesPorSolicitud.NotificacionesPorSolicitudClient().UpdatePreferenciasPipeline(Operacion, TipoidCliente, IdCliente, Sms, Mail, User, NoSeleccionoRecibirViaSms, NoSeleccionoRecibirViaEmail);
                this._logger.Info($" NotificacionesPorSolicitud.Administracion_Mexico > UpdatePreferenciasPipeline > Datos entrada: {JsonConvert.SerializeObject(new object[] { operacion, tipoidCliente, idCliente, sms, mail, user, NoSeleccionoRecibirViaSms, NoSeleccionoRecibirViaSms, NoSeleccionoRecibirViaEmail })}");
                new Data.AdministracionNotificacionesMexico.UpdatePreferenciasPipeline().Run(new object[] { operacion, tipoidCliente, idCliente, sms, mail, user, NoSeleccionoRecibirViaSms, NoSeleccionoRecibirViaSms, NoSeleccionoRecibirViaEmail });
            }
            catch (Exception ex)
            {
                this._logger.Info($"NotificacionesPorSolicitud.Administracion_Mexico > UpdatePreferenciasPipeline (error) : {ex.Message}");
                throw;
            }
        }
    }
}
