using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using SkCo.Notificaciones.DSL;
using System.ServiceModel.Web;

namespace OM.Notifications.Facade
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INotificacionesPorSolicitud" in both code and config file together.
    [ServiceContract]
    public interface INotificacionesPorSolicitud
    {
        /// <summary>
        /// Inserta un mensaje para enviar al MSMQ
        /// </summary>
        /// <param name="idMensaje"></param>
        /// <param name="tipoIdCliente"></param>
        /// <param name="idCliente"></param>
        /// <param name="contrato"></param>
        /// <param name="contacto"></param>
        [OperationContract]
        void EnqueueMessage(int idCategoria, string tipoIdCliente, string numIdCliente, string contrato, string monto, string NroConfirmacion, string IP, string Descripcion, string producto);

        /// <summary>
        /// Inserta un mensaje para enviar al MSMQ
        /// </summary>
        /// <param name="idMensaje"></param>
        /// <param name="tipoIdCliente"></param>
        /// <param name="idCliente"></param>
        /// <param name="contrato"></param>
        /// <param name="contacto"></param>
        [OperationContract(IsOneWay = true)]
        void MSMQMessage(int idCategoria, string tipoIdCliente, string numIdCliente, string contrato, string monto, string producto, DSNotificaciones MensajesAdicionales);

        [OperationContract]
        string MSMQMessage_Mexico(int Operacion, string tipoIdCliente, string numIdCliente, string contrato, decimal monto, string mobilCliente, string mobilFP, string emailCliente, string emailFP, DSNotificaciones MensajesAdicionales, out string detallError);

        // <<<<<<<< Metodos antiguos que se reporatron en  gemini ADM01-199718 >>>>>>>>
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void C_E_comunicacion_Automatica_insert(string Campana, string Documento, string Tipo_Doc, string email, string campo1_text);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        System.Data.DataSet ConsultarMensajes_PreferenciasTransaccion(string Documento, string Tipo_Documento, int Pais);

        [OperationContract]
        System.Data.DataSet ConsultarPreferencias(int Pais);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string CrearPreferenciasTyCMx(int Pais, string user, string ClienteRFC, string Movil, string contrato, string tipoTerminos);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void EnqueueEmail_Mexico(int IdTabla, string Value);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void Notificacion_SMS_CreacionCuentas(string Producto, string Plan, string contrato);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string NotificarTyCMx(int Pais, string user, string ClienteRFC, string Movil, string contrato, int tipoOperacion);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string PreferenciasEditor(string PERS_IdNumber, string PERS_IdType, int IdMensaje, string UsuarioCrea, string UsuarioModifica, bool Sms, bool Email, string categoria, string contrato, string pais, string strRespuesta, string Movil);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string PreferenciasEditorEditar(string PERS_IdNumber, bool Sms, bool Email, string categoria);


        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        bool VerificarPreferenciasTyCMx(int Pais, string user, string ClienteRFC, string Movil, string contrato, string tipoTerminos);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void UpdatePreferenciasPipeline(int operacion, string tipoidCliente, string idCliente, bool sms, bool mail, string user, bool NoSeleccionoRecibirViaSms, bool NoSeleccionoRecibirViaEmail);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml,
         ResponseFormat = WebMessageFormat.Xml,
         BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void UpdatePreferenciasPortal(int operacion, string tipoidCliente, string idCliente, bool sms, bool mail, string user);
    }
}
