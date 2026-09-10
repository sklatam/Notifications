using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OM.Notifications.Processors.BizTalkProxy.Impl;
using OM.Notifications.Processors.Factory;

namespace OM.Notifications.Processors.BizTalkProxy
{
    public class WebServiceMessageProcessorFactory : IMessageProcessorFactory
    {
        public IMessageProcessor BuildProcessor(bool isOnlineMessage)
        {
            return new LegacyMsmqMessageProcessor();
        }
    }
}
