using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using OM.Notifications.Data.DataHelpers;
using NLog;

namespace OM.Notifications.Processors.DataHelpers
{
    public class MessageStatusHelper
    {
        // Utilidades del logger
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// /No cumple NoCumpleReintento
        /// </summary>
        /// <param name="idMensaje"></param>
        /// <param name="tipoIdCliente"></param>
        /// <param name="idCliente"></param>
        /// <param name="contrato"></param>
        /// <returns></returns>
        public void MarkNotCurrent(long messageId)
        {
            this._logger.Info($":NoCumpleVigenciaMensaje, Se invoca el metodo NoCumpleValidacionOnline Id = {messageId}");

            try
            {
                //se actualiza con el estado del mensaje
                new MarkSentSupplierRunner().Run(messageId, Constants.MessageBillingNoEnviado, string.Empty, Constants.NoEnviado);
            }
            catch (Exception exception)
            {
                this._logger.Error($"NoCumpleVigenciaMensaje => {exception}");
            }
        }

        /// <summary>
        /// /No cumple horario de envio
        /// </summary>
        /// <param name="idMensaje"></param>
        /// <param name="tipoIdCliente"></param>
        /// <param name="idCliente"></param>
        /// <param name="contrato"></param>
        /// <returns></returns>
        public void MarkNotInSchedule(long messageId)
        {
            this._logger.Info($":NoCumpleHoraEnvio, Se invoca el metodo NoCumpleHoraEnvio Id = {messageId}");

            try
            {
                new MarkSentSupplierRunner().Run(messageId, Constants.MessageBillingNoEnviado, string.Empty, Constants.NoEnviado);
            }
            catch (Exception exception)
            {
                this._logger.Error($"NoCumpleHoraEnvio => {exception}");
            }
        }

        /// <summary>
        /// No cumple regla de envio posibles causas:
        /// Monto
        /// Contrato
        /// Fecha
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="Token"></param>
        public void MarkNotFulfillsRule(long messageId)
        {
            this._logger.Info($":NoCumpleReglaParametrizada, Se invoca el metodo NoCumpleReglaParametrizada Id = {messageId}");

            try
            {
                new MarkSentRunner().Run(messageId, Constants.NoCumpleReglaEnvio, Constants.MessageBillingRuleEnvio);
            }
            catch (Exception exception)
            {
                this._logger.Error($"NoCumpleReglaParametrizada, {exception}");
            }
        }
        
        /// <summary>
        /// Actualizar Estado del mensaje
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="billingCode"></param>
        /// <param name="token"></param>
        public void MarkSent(long messageId, int billingCode, string token)
        {
            this._logger.Info($"Se invoca el metodo marcarenviadoProveedor con parametros:: >>CodeBilling {billingCode} >>IdTablaEnvioMasivo {messageId} >>Token {token}");
            
            try
            {
                this._logger.Info($"IdTablaEnvioMasivo {messageId} >> CodeBilling {billingCode} >> Token {token}");
                // Actualizar Estado del mensaje
                new MarkSentSupplierRunner().Run(
                    messageId, 
                    billingCode,
                    token, 
                    Constants.ENVIADOALOPERADOR);
            }
            catch (Exception exception)
            {
                this._logger.Error($"MarcarenviadoProveedor >> Exception => {exception}");
            }
        }

        /// <summary>
        /// Error Generico
        /// </summary>
        /// <param name="messageId"></param>
        public void MarkError(long messageId)
        {
            try
            {
                // Actualizar Estado del mensaje
                new MarkSentSupplierRunner().Run(messageId, Constants.ErrorMessageBilling, string.Empty, Constants.Error);
            }
            catch (Exception ex)
            {
                this._logger.Fatal("Error marcando mensaje en Error:" + ex);
            }
        }
    }
}

