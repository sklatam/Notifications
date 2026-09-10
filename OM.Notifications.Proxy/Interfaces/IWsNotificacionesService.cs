using OM.Notifications.Entities;

namespace OM.Notifications.Facade.Interfaces
{
    public interface IWsNotificacionesService
    {
        void SendMessage(
            int operationId,
            string docNumber,
            string docType,
            int contractNumber,
            string productCode,
            decimal amount,
            MessageVariable[] variables,
            bool? sendToFpOnly);
    }
}
