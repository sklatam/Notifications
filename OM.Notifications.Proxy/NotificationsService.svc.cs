using OM.Common.Utils.WcfClient;
using System;
using System.Collections.Generic;
using OM.Notifications.Entities;
using OM.Notifications.BLL;
using NLog;
using System.ServiceModel;
using Newtonsoft.Json;
using OM.Notifications.Facade.Services;
using OM.Notifications.Data;

namespace OM.Notifications.Facade
{
    /// <summary>
    /// Esta clase implementa la interfaz del servicio MIGRADA a la nueva arquitectura (OM.Notifications) en su versión Legacy (usando BizTalk).
    /// 
    /// Según los parámetros de uso de proxy (Settings) se comporta como proxy o como servicio real.
    /// Cuando se comporta como servicio real asume que existen las colas de mensajes para enviar a BizTalk.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class NotificationsService : INotificationsService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public NotificationsService()
        {
            Mapping.MappingConfig.Configure();
        }

        public void SendMessage(int operationId, string docNumber, string docType, int contractNumber, string productCode, decimal amount, MessageVariable[] variables, bool? sendToFpOnly)
        {
            try
            {
                Services.Factory.Build(Properties.Settings.Default.UseProxyForMessages)
                    .SendMessage(operationId, docNumber, docType, contractNumber, productCode, amount, variables, sendToFpOnly);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                throw;
            }
        }

        public void SendMessageWithAttachments(int operationId, string from, string to, string subject, MessageVariable[] variables, MessageAttachment[] attachments, MessageContact[] contactsBcc = null)
        {
            try
            {
                Services.Factory.Build(Properties.Settings.Default.UseProxyForEmail)
                    .SendMessageWithAttachments(operationId, from, to, subject, variables, attachments, contactsBcc);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                throw;
            }
        }

        public void SendMessageWithContacts(int operationId, string docNumber, string docType, int contractNumber, decimal amount, MessageVariable[] variables, MessageContact[] contacts)
        {
            try
            {
                Services.Factory.Build(Properties.Settings.Default.UseProxyForMessages)
                    .SendMessageWithContacts(operationId, docNumber, docType, contractNumber, amount, variables, contacts);
            }
            catch (Exception ex)
            {
                this._logger.Error(ex.ToString());
                throw;
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
