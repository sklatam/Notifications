using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper;
using OM.Common.Utils.EnumDescription;

namespace OM.Notifications.Startup
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            Mapper.Reset();

            Mapper.Register<Data.DataModels.MessageRecord, Processors.Message>()
                .Member(t => t.Body, s => s.MessageBody)
                .Member(t => t.Recipient, s => EnumDescriptor.GetValue<Processors.MessageRecipient>(s.RecipientType))
                .Member(t => t.Type, s => EnumDescriptor.GetValue<Processors.MessageType>(s.MessageType))
                .Value(t => t.IsOnline, false);

            Mapper.Compile();
        }
    }
}
