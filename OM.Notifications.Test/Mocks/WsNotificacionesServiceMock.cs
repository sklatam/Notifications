using Moq;
using OM.Notifications.Facade.Interfaces;

namespace OM.Notifications.Test.Mocks
{
    internal static class WsNotificacionesServiceMock
    {
        public static Mock<IWsNotificacionesService> Create()
        {
            return new Mock<IWsNotificacionesService>(MockBehavior.Strict);
        }
    }
}
