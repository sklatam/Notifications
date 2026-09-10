using Moq;
using NUnit.Framework;
using OM.Notifications.Entities;
using OM.Notifications.Facade.Services;
using OM.Notifications.Test.Mocks;
using OM.Notifications.Test.Stubs;

namespace OM.Notifications.Test.Services
{
    [TestFixture]
    public class WsProxyServiceTests
    {
        [Test]
        public void SendMessage_Should_Remove_Null_Variables_Before_Sending()
        {
            // Arrange
            var wsMock = WsNotificacionesServiceMock.Create();

            MessageVariable[] variablesReceivedByWs = null;

            wsMock
                .Setup(x => x.SendMessage(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<MessageVariable[]>(),
                    It.IsAny<bool?>()))
                .Callback<int, string, string, int, string, decimal, MessageVariable[], bool?>(
                    (operationId,
                     docNumber,
                     docType,
                     contractNumber,
                     productCode,
                     amount,
                     vars,
                     sendToFpOnly) =>
                    {
                        variablesReceivedByWs = vars;
                    });


            var service = new WsProxyService("http://fake-endpoint", wsMock.Object);

            var variables = new MessageVariable[]
            {
                MessageVariableStub.Create("Valor", "393381"),
                null,
                MessageVariableStub.Create("ValorPrimaMensual", "393382")
            };

            // Act
            service.SendMessage(364,"53140669","C",36728,"OMPEV",393381,variables,false);

            // Assert
            Assert.That(variablesReceivedByWs, Is.Not.Null);
            Assert.That(variablesReceivedByWs.Length, Is.EqualTo(2));
            Assert.That(variablesReceivedByWs, Has.None.Null);

            wsMock.Verify(
                x => x.SendMessage(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<MessageVariable[]>(),
                    It.IsAny<bool?>()),
                Times.Once);
        }
    }
}
