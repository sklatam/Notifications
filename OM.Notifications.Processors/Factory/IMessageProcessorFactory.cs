using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OM.Notifications.Processors.Factory
{
    public interface IMessageProcessorFactory
    {
        IMessageProcessor BuildProcessor(bool isOnlineMessage);
    }
}
