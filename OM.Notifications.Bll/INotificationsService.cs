using OM.Notifications.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace OM.Notifications.BLL
{
    [ServiceContract]

    public interface INotificationsService
    {
        [OperationContract]
        void SendMessage(int operationId, string docNumber, string docType, int contractNumber, string productCode, decimal amount, MessageVariable[] variables, bool? sendToFpOnly);

        [OperationContract]
        void SendMessageWithAttachments(int operationId, string from, string to, string subject, MessageVariable[] variables, MessageAttachment[] attachments, MessageContact[] contactsBcc = null);

        [OperationContract]
        void SendMessageWithContacts(int operationId, string docNumber, string docType, int contractNumber, decimal amount, MessageVariable[] variables, MessageContact[] contacts);

        [OperationContract]
        string GetMessageTemplate(int operationId);
    }
}
