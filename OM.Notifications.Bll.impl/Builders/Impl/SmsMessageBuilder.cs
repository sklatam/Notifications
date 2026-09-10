using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NLog;
using OM.Notifications.BLL.Impl.Helpers;
using OM.Notifications.Entities;
using OM.Notifications.Processors;

namespace OM.Notifications.BLL.Impl.Builders.Impl
{
    internal class SmsMessageBuilder : IMessageBuilder
    {
        private readonly bool _sendToFpOnly;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public SmsMessageBuilder(bool sendToFpOnly)
        {
            this._sendToFpOnly = sendToFpOnly;
        }

        public IEnumerable<Message> BuildMessages(int operationId, string docNumber, string docType, int contractNumber, string productCode, decimal amount, bool sendCopyFp, int operationFpId, MessageContact[] contacts, Entities.MessageVariable[] variables)
        {
            if (!this._sendToFpOnly)
            {
                // Se obtienen los datos del CLIENTE destinatario
                var contact = contacts.FirstOrDefault(c => c.Recipient.Equals(ContactRecipient.Customer) && c.Type.Equals(Entities.MessageType.SMS));
                if (contact != null)
                {
                    var message = new Processors.Message
                    {
                        Recipient = Processors.MessageRecipient.Customer,
                        Type = Processors.MessageType.SMS,
                        OperationId = operationId,
                        DocType = docType,
                        DocNumber = docNumber,
                        ContractNumber = string.IsNullOrEmpty(contact?.UnifiedReference) ? "No aplica" : contact.UnifiedReference,
                        Body = string.IsNullOrEmpty(contact?.MessageBody) ? "No tiene configurado un cuerpo de Mensaje" : contact.MessageBody,
                        Contact = string.IsNullOrEmpty(contact?.Contact) ? "No tiene" : contact.Contact,
                        IsOnline = contact != null && contact.IsOnline
                    };

                    
                    // Parametros fijos
                    message.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroContrato"], contractNumber.ToString());
                    message.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroValor"], amount.ToString());
                    message.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroFecha"], DateTime.Today.ToString("yyyyMMdd"));

                    // Parametros adicionales
                    message.AddVariables(variables);

                    this._logger.Debug($"Customer message: " + JsonConvert.SerializeObject(message));

                    yield return message;
                }
            }

            if (sendCopyFp)
            {
                // Se obtienen los datos del FP destinatario
                var contact = contacts.FirstOrDefault(c => c.Recipient.Equals(ContactRecipient.Agent) && c.Type.Equals(Entities.MessageType.SMS));
                if (contact != null)
                {
                    var messageFp = new Processors.Message
                    {
                        Recipient = Processors.MessageRecipient.Agent,
                        Type = Processors.MessageType.SMS,
                        OperationId = operationFpId,
                        DocType = docType,
                        DocNumber = docNumber,
                        ContractNumber = string.IsNullOrEmpty(contact?.UnifiedReference) ? "No aplica" : contact.UnifiedReference,
                        Body = string.IsNullOrEmpty(contact?.MessageBody) ? "No tiene configurado un cuerpo de Mensaje" : contact.MessageBody,
                        Contact = string.IsNullOrEmpty(contact?.Contact) ? "No tiene" : contact.Contact,
                        IsOnline = contact != null && contact.IsOnline
                    };

                    // Parametros fijos
                    messageFp.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroContrato"], contractNumber.ToString());
                    messageFp.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroValor"], amount.ToString());
                    messageFp.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroFecha"], DateTime.Today.ToString("yyyy-MM-dd"));

                    // Parametros adicionales
                    messageFp.AddVariables(variables);

                    this._logger.Debug($"Agent message: " + JsonConvert.SerializeObject(messageFp));

                    yield return messageFp;
                }
            }
        }
    }
}
