using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ExpressMapper;
using OM.Notifications.Processors.BizTalkProxy.Entities;

namespace OM.Notifications.Processors.BizTalkProxy.Impl
{
    public class LegacyMsmqMessageProcessor : IMessageProcessor
    {
        private readonly string _msmqPath = OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["MSMQPath"];

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public void ProcessMessage(Message message)
        {
            this._logger.Info($"Processing message: {JsonConvert.SerializeObject(message)}");

            var legacyMessage = Mapper.Map<Message, NotificacionesRequest>(message);

            // Serializamos el XML
            var sb = new StringBuilder();
            new XmlSerializer(legacyMessage.GetType())
                .Serialize(XmlWriter.Create(sb), legacyMessage);

            var xmlMessage = new XmlDocument();
            xmlMessage.LoadXml(sb.ToString());
            
            var theMessage = new System.Messaging.Message(xmlMessage)
            {
                Label = $"Msj to: {message.DocType}{message.DocNumber}",
                Priority = System.Messaging.MessagePriority.Low,
            };

            this._logger.Info($"Send legacy message to Queue: {xmlMessage.OuterXml}");
            new System.Messaging.MessageQueue(this._msmqPath)
                .Send(theMessage);
        }
    }
}
