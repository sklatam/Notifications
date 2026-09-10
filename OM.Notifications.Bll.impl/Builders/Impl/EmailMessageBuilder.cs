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
    internal class EmailMessageBuilder : IMessageBuilder
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public IEnumerable<Message> BuildMessages(int operationId, string docNumber, string docType, int contractNumber, string productCode, decimal amount, bool sendCopyFp, int operationFpId, MessageContact[] contacts, Entities.MessageVariable[] variables)
        {
            // Se obtienen los datos del CLIENTE destinatario
            var contact = contacts.FirstOrDefault(c => c.Recipient.Equals(ContactRecipient.Customer) && c.Type.Equals(Entities.MessageType.Email));
            if (contact != null)
            {
                var message = new Processors.Message
                {
                    Recipient = Processors.MessageRecipient.Customer,
                    Type = Processors.MessageType.Email,
                    OperationId = operationId,
                    DocType = docType,
                    DocNumber = docNumber,
                    ContractNumber = string.IsNullOrEmpty(contact?.UnifiedReference) ? "No aplica" : contact.UnifiedReference,
                    Contact = string.IsNullOrEmpty(contact?.Contact) ? "No tiene" : contact.Contact,
                    ProductCode = string.IsNullOrEmpty(productCode) ? string.Empty : productCode,
                    FirstName = string.IsNullOrEmpty(contact?.FirstName) ? string.Empty : contact.FirstName,
                    LastName = string.IsNullOrEmpty(contact?.LastName) ? string.Empty : contact.LastName,

                    // Para un tipo de mensaje email no se configura un cuerpo de mensaje en el sistema de notificaciones
                    // la configuracion se realiza en el sistema campaing(HTML Retroalimentado por el area de comunicaciones)
                    Body = string.IsNullOrEmpty(contact?.MessageBody) ? "No aplica" : contact.MessageBody,
                    IsOnline = contact != null && contact.IsOnline
                };

                // Parametros - El orden de insercion determina el campo al que se asigna en Campaign
                // Se dejan los explicitos primero para no generar cambios inesperados respecto a lo que espera la plantilla

                // 1. Parametros explicitos
                message.AddVariables(variables);

                // 2. Parametros adicionales 
                message.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroContrato"], contractNumber.ToString());
                message.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroValor"], amount.ToString());
                message.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroFecha"], DateTime.Today.ToString("yyyy-MM-dd"));
                message.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroProducto"], message.ProductCode);
                message.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroNombres"], message.FirstName);
                message.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroApellidos"], message.LastName);
                
                this._logger.Debug($"Customer message: " + JsonConvert.SerializeObject(message));

                yield return message;
            }

            if (sendCopyFp)
            {
                contact = contacts.FirstOrDefault(c => c.Recipient.Equals(ContactRecipient.Agent) && c.Type.Equals(Entities.MessageType.Email));
                if (contact != null)
                {
                    var messageFp = new Processors.Message
                    {
                        Recipient = Processors.MessageRecipient.Agent,
                        Type = Processors.MessageType.Email,
                        OperationId = operationFpId,
                        DocType = docType,
                        DocNumber = docNumber,
                        ContractNumber = string.IsNullOrEmpty(contact?.UnifiedReference) ? "No aplica" : contact.UnifiedReference,
                        Body = string.IsNullOrEmpty(contact?.MessageBody) ? "No tiene configurado un cuerpo de Mensaje" : contact.MessageBody,
                        Contact = string.IsNullOrEmpty(contact?.Contact) ? "No tiene" : contact.Contact,
                        IsOnline = contact != null && contact.IsOnline
                    };

                    // Parametros - El orden de insercion determina el campo al que se asigna en Campaign

                    // 1. Parametros explicitos
                    messageFp.AddVariables(variables);

                    // 2. Parametros adicionales
                    messageFp.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroContrato"], contractNumber.ToString());
                    messageFp.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroValor"], amount.ToString());
                    messageFp.AddVariable(OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["NameParametroFecha"], DateTime.Today.ToString("yyyy-MM-dd"));
                    
                    this._logger.Debug($"Agent message: " + JsonConvert.SerializeObject(messageFp));

                    yield return messageFp;
                }
            }
        }
    }
}
