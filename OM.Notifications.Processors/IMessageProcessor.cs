using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OM.Common.Utils.EnumDescription;

namespace OM.Notifications.Processors
{
    public interface IMessageProcessor
    {
        void ProcessMessage(Message message);
    }

    public class Message
    {
        public int Id { get; set; }

        public int OperationId { get; set; }

        public DateTime ProcessDate { get; set; }

        public MessageType Type { get; set; }

        public MessageRecipient Recipient { get; set; }

        public bool IsOnline { get; set; }

        public string Contact { get; set; }

        public string Body { get; set; }

        public string DocType { get; set; }

        public string DocNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ContractNumber { get; set; }
        
        public string ProductCode { get; set; }

        public string Value { get; set; }

        public IList<Entities.MessageVariable> Variables { get; set; }
    }

    public enum MessageType
    {
        [EnumDescription("SMS")]
        SMS,

        [EnumDescription("Email")]
        Email
    }

    public enum MessageRecipient
    {
        [EnumDescription("CLIENTE")]
        Customer,

        [EnumDescription("FP")]
        Agent
    }
}
