using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OM.Notifications.BLL.Impl.Builders
{
    public class MessageBuilderFactory
    {
        private const string SmsMessageType = "SMS";
        private const string EmailMessageType = "Email";

        public IMessageBuilder GetFor(string messageType, bool? sendToFpOnly, out string messageTypeId, out bool shouldSaveVariables)
        {
            if (messageType == SmsMessageType)
            {
                messageTypeId = "1";
                shouldSaveVariables = false;

                return new Builders.Impl.SmsMessageBuilder(sendToFpOnly.GetValueOrDefault());
            }
            else if (messageType == EmailMessageType)
            {
                messageTypeId = "2";
                shouldSaveVariables = true;

                return new Builders.Impl.EmailMessageBuilder();
            }
            else
            {
                throw new Exception("No se conoce el tipo de mensaje " + messageType);
            }
        }
    }
}
