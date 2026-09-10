using OM.Common.Utils.EnumDescription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OM.Notifications.Entities
{
    public class MessageContact
    {
        public string UnifiedReference { get; set; }

        public string Contact { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsOnline { get; set; }

        public string MessageBody { get; set; }

        public ContactRecipient Recipient { get; set; }

        public MessageType Type { get; set; }

    }

    public enum ContactRecipient
    {
        [EnumDescription("CLIENTE")]
        Customer,

        [EnumDescription("FP")]
        Agent
    }

    public enum MessageType
    {
        [EnumDescription("SMS")]
        SMS,

        [EnumDescription("Email")]
        Email
    }
}
