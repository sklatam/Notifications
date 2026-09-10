using Newtonsoft.Json;
using OM.Notifications.Data;
using OM.Notifications.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using NLog;
using OM.Common.Utils.EnumDescription;
using OM.Notifications.BLL.Impl.Helpers;

namespace OM.Notifications.BLL.Impl
{
    public class NotificationsService : INotificationsService
    {
        private readonly Builders.MessageBuilderFactory _messageBuilderFactory;
        private readonly Processors.Factory.IMessageProcessorFactory _messageProcessorFactory;
        private readonly Processors.Services.IMailService _mailService;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly string _xmlFieldNames;

        public NotificationsService(
            Builders.MessageBuilderFactory messageBuilderFactory,
            Processors.Factory.IMessageProcessorFactory messageProcessorFactory,
            Processors.Services.IMailService mailService)
        {
            this._messageBuilderFactory = messageBuilderFactory;
            this._messageProcessorFactory = messageProcessorFactory;
            this._mailService = mailService;

            this._xmlFieldNames = OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroXml"];
            if (string.IsNullOrEmpty(this._xmlFieldNames))
                this._xmlFieldNames = "xml,html,campo1_xml";
        }

        public void SendMessage(int operationId, string docNumber, string docType, int contractNumber, string productCode, decimal amount, MessageVariable[] variables, bool? sendToFpOnly)
        {
            this._logger.Info($"Sending message: {JsonConvert.SerializeObject(new { operationId = operationId, docNumber = docNumber, docType = docType, productCode = productCode, contractNumber = contractNumber, amount = amount, variables = variables, sendToFpOnly = sendToFpOnly })}");

            try
            {
                var dateSms = variables?.FirstOrDefault(e => e.Name == OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroSMSFecha"])?.Value;
                // CONSULTA LOS DATOS REQUERIDOS PARA ENVIAR MENSAJE POR SMS Y/O EMAIL     
                // SMSFECHA ES EL REEMPLAZO DE LA FECHA ACTUAL QUE SE DEJA EN EL MENSAJE
                var messagesData = new ConsultarByMensajeCategoria().Get(operationId, contractNumber.ToString(), docNumber, productCode, amount, dateSms);

                // CIGA: -------------------------------------------------BLOQUE DE VALIDACIONES ---------------------------------------------------------------------
                //Validar si existe una validación para eso operación, incluye categorias padres.
                var validaciones = new ConsultarValidacionByOperacacion().Get(operationId);

                // Si existe una validación, se procesa
                foreach (var dr in validaciones.Tables[0].Rows
                    .OfType<DataRow>())
                {
                    this._logger.Info($"Ejecuta Validacion: EXEC " + dr["Validacion"].ToString());
                    // Si la validación no debe aplicar sobre esa operaciónm, se salta la validación
                    if (dr["validacion"].ToString().ToUpper().Equals("NA") || dr["validacion"].ToString().ToUpper().Equals("NULL"))
                        break;
                    var result = new EjecutarValidacion(dr["Validacion"].ToString()).Get(operationId, docNumber, docType, contractNumber, productCode, amount);
                    bool enviar = bool.Parse(result.Tables[0].Rows[0]["Enviar"].ToString());

                    if (enviar)
                    {
                        var tipomensaje = result.Tables[0].Rows[0]["TipoMensaje"].ToString();
                        var newMessagesData = new DataSet();
                        newMessagesData.Merge(messagesData.Tables[0].Select("TipoMensajeCliente IN (" + tipomensaje + ")"));
                        messagesData = newMessagesData;
                    }
                    else
                    {
                        this._logger.Info($"El mensaje no fue enviado luego de evaluar la validación ");
                        return;
                    }
                }
                /// ---------------------------------------------TERMINA VALIDACIONES ---------------------------------------------------------------------------

                foreach (var dr in messagesData.Tables[0].Rows
                    .OfType<DataRow>())
                {
                    string messageTypeId;
                    bool shouldSaveVariables;

                    var messageBuilder = this._messageBuilderFactory.GetFor(dr["TipoMensajeCliente"].ToString(), sendToFpOnly, out messageTypeId, out shouldSaveVariables);
                    var messages = messageBuilder.BuildMessages(operationId, docNumber, docType, contractNumber, productCode, amount, Convert.ToBoolean(dr["CopiaFP"]), dr.IsNull("IdOperacionFP") ? 0 : Convert.ToInt32(dr["IdOperacionFP"]), dr.GetContacts(), variables);
                    this.ProcessMessages(messages, messageTypeId, shouldSaveVariables);
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                throw new Exception("Ocurrió un error al procesar la notificación");
            }
        }

        public void SendMessageWithContacts(int operationId, string docNumber, string docType, int contractNumber, decimal amount, MessageVariable[] variables, MessageContact[] contacts)
        {
            this._logger.Info($"Sending message: {JsonConvert.SerializeObject(new { operationId = operationId, docNumber = docNumber, docType = docType, contractNumber = contractNumber, amount = amount, variables = variables, contacts = contacts })}");

            try
            {
                // CONSULTA LOS DATOS REQUERIDOS PARA ENVIAR MENSAJE POR SMS Y/O EMAIL     
                var messagesData = new ConsultarByMensajeCategoriaMx().Get(operationId, contractNumber.ToString(), amount);
                foreach (var dr in messagesData.Tables[0].Rows
                    .OfType<DataRow>())
                {
                    string messageTypeId;
                    bool shouldSaveVariables;

                    var messageBuilder = this._messageBuilderFactory.GetFor(dr["TipoMensajeCliente"].ToString(), false, out messageTypeId, out shouldSaveVariables);
                    var messages = messageBuilder.BuildMessages(operationId, docNumber, docType, contractNumber, string.Empty, amount, Convert.ToBoolean(dr["CopiaFP"]), dr.IsNull("IdOperacionFP") ? 0 : Convert.ToInt32(dr["IdOperacionFP"]), dr.GetContacts(contacts).ToArray(), variables);

                    this.ProcessMessages(messages, messageTypeId, shouldSaveVariables);
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                throw new Exception("Ocurrió un error al procesar la notificación");
            }
        }

        public void SendMessageWithAttachments(int operationId, string from, string to, string subject, MessageVariable[] variables, MessageAttachment[] attachments, MessageContact[] contactsBcc = null)
        {
            this._logger.Info($"Sending message: {JsonConvert.SerializeObject(new { operationId = operationId, from = from, to = to, variables = variables, contactsBcc = contactsBcc })}");

            try
            {
                var body = new ObtenerCuerpoMensajePorOperacion()
                    .Get(operationId);

                if (string.IsNullOrEmpty(body))
                    this._logger.Warn($"No se encontró cuerpo de mensaje para el templateId = {operationId}");

                this._mailService.SendEmail(from, to, subject, body, variables, attachments.SecureContent(), contactsBcc);

                try
                {
                    string docNumber = variables?.Where(c => c.Name == "DocNumber").Select(c => c.Value).FirstOrDefault();
                    new CrearLogMensajesAttachment().Get(operationId, from, to, subject, JsonConvert.SerializeObject(variables ?? new object()), attachments == null ? 0 : attachments.Count(), docNumber);
                }
                catch (Exception logEx)
                {
                    this._logger.Warn("Error guardando log de envío: " + logEx);
                }
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                throw new Exception("Ocurrió un error al enviar el mensaje");
            }
        }

        private void ProcessMessages(IEnumerable<Processors.Message> messages, string messageTypeId, bool shouldSaveVariables)
        {
            var messageData = new CrearEnvioMasivoMensajesPorSolicitud();
            var variablesData = new EnvioMasivoMensajesAdicionalesInsert();

            foreach (var message in messages)
            {
                message.Id = messageData.Get(
                    message.DocType,
                    message.DocNumber,
                    message.OperationId,
                    message.Body,
                    message.Contact,
                    message.ContractNumber,
                    message.GetParameter(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroValor"]),
                    true,
                    EnumDescriptor.GetDescription(message.Recipient),
                    messageTypeId);

                if (shouldSaveVariables)
                {
                    var xmlFieldNameList = this._xmlFieldNames.Split(',');
                    foreach (var item in message.Variables)
                    {
                        string standardValue = item.Value;
                        string xmlValue = null;

                        if (xmlFieldNameList.Any(f => string.Equals(item.Name, f, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            xmlValue = item.Value;
                            standardValue = null;
                        }

                        variablesData.Run(new object[] { message.Id, item.Name, standardValue, xmlValue });
                    }
                }

                this._logger.Debug($"Número de IdTablaMasivo {messageTypeId} {message.Recipient}: {message.Id}");

                // Determina el processor para el mensaje, segun el campo "EnvioEnLinea" del tipo de mensaje (si existe)
                var processor = this._messageProcessorFactory.BuildProcessor(message.IsOnline);
                processor.ProcessMessage(message);
            }
        }

        public string GetMessageTemplate(int operationId)
        {
            this._logger.Info($"Get Message Template: {operationId}");
            string htmlTemplate = string.Empty;

            try
            {
                htmlTemplate = new ObtenerCuerpoMensajePorOperacion().Get(operationId);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                this._logger.Error($"Ocurrió un error al procesar :::: Get Message Template: {operationId}");
            }

            return htmlTemplate;
        }
    }
}
