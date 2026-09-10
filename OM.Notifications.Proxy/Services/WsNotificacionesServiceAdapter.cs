using OM.Notifications.Entities;
using OM.Notifications.Facade.Interfaces;
using OM.Notifications.Facade.WsNotificationsService;
using System.ServiceModel;
using OM.Common.Utils.WcfClient;

namespace OM.Notifications.Facade.Services
{
    public class WsNotificacionesServiceAdapter : IWsNotificacionesService
    {
        private readonly string _serviceEndpoint;

        public WsNotificacionesServiceAdapter(string serviceEndpoint)
        {
            _serviceEndpoint = serviceEndpoint;
        }

        public void SendMessage(int operationId, string docNumber, string docType, int contractNumber, string productCode, decimal amount, MessageVariable[] variables, bool? sendToFpOnly)
        {
            new NotificationsServiceClient(
                 new BasicHttpBinding(),
                 new EndpointAddress(_serviceEndpoint))
                 .Invoke(c => c.SendMessage(
                     operationId,
                     docNumber,
                     docType,
                     contractNumber,
                     productCode,
                     amount,
                     variables,
                     sendToFpOnly
                 ));
        }
    }
}
