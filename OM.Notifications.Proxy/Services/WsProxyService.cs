using System;
using System.Linq;
using Newtonsoft.Json;
using NLog;
using OM.Common.Utils.WcfClient;
using OM.Notifications.Entities;
using OM.Notifications.Facade.Interfaces;

namespace OM.Notifications.Facade.Services
{
    public class WsProxyService : BLL.INotificationsService
    {
        private readonly string _serviceEndpoint;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly IWsNotificacionesService _wsClient;

        //Constructor para realizar pruebas unitarias 
        public WsProxyService(string serviceEndpoint, IWsNotificacionesService wsClient)
        {
            _serviceEndpoint = serviceEndpoint;
            _wsClient = wsClient;
        }

        public WsProxyService(string serviceEndpoint)
        {
            this._serviceEndpoint = serviceEndpoint;
            _wsClient = new WsNotificacionesServiceAdapter(serviceEndpoint);
        }        

        public void SendMessage(int operationId, string docNumber, string docType, int contractNumber, string productCode, decimal amount, MessageVariable[] variables, bool? sendToFpOnly)
        {
            // Se implementa saneamiento del arreglo MessageVariable[] debido a envíos con elementos null
            int originalCount = variables?.Length ?? 0;

            if (variables != null && variables.Any(v => v == null))
            {
                variables = variables
                    .Where(v => v != null)
                    .ToArray();

                _logger.Warn($"WsProxyService.SendMessage sanitized variables | " + 
                    $"RemovedNulls={originalCount - variables.Length} | " +
                    $"OperationId={operationId} | Doc={docNumber} | Contract={contractNumber}"
                );
            }

            if (variables == null || variables.Length == 0)
            {
                _logger.Warn(
                    $"WsProxyService.SendMessage received empty variables | " +
                    $"OperationId={operationId} | Doc={docNumber} | Contract={contractNumber}"
                );
            }            

            this._logger.Info($"Sending via Proxy ({this._serviceEndpoint}): " + JsonConvert.SerializeObject(new{operationId, docNumber, docType, contractNumber, amount, variables}) );

            _wsClient.SendMessage(operationId, docNumber, docType, contractNumber, productCode, amount, variables, sendToFpOnly);
        }


        public void SendMessageWithAttachments(int operationId, string from, string to, string subject, MessageVariable[] variables, MessageAttachment[] attachments, MessageContact[] contactsBcc = null)
        {
            this._logger.Info($"Sending via Proxy ({this._serviceEndpoint}): {JsonConvert.SerializeObject(new { operationId, from, to, subject, variables, contactsBcc })}");

            new WsNotificationsService.NotificationsServiceClient(
                new System.ServiceModel.BasicHttpBinding(),
                new System.ServiceModel.EndpointAddress(this._serviceEndpoint))
                .Invoke(c => c.SendMessageWithAttachments(operationId, from, to, subject, variables, attachments, contactsBcc));
        }

        public void SendMessageWithContacts(int operationId, string docNumber, string docType, int contractNumber, decimal amount, MessageVariable[] variables, MessageContact[] contacts)
        {
            this._logger.Info($"Sending via Proxy ({this._serviceEndpoint}): {JsonConvert.SerializeObject(new { operationId, docNumber, docType, contractNumber, amount, variables, contacts })}");

            new WsNotificationsService.NotificationsServiceClient(
                new System.ServiceModel.BasicHttpBinding(),
                new System.ServiceModel.EndpointAddress(this._serviceEndpoint))
                .Invoke(c => c.SendMessageWithContacts(operationId, docNumber, docType, contractNumber, amount, variables, contacts));
        }

        public string GetMessageTemplate(int operationId)
        {
            throw new NotImplementedException();
        }
    }
}