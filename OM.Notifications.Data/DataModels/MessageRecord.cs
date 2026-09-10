using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OM.Notifications.Data.DataModels
{
    public class MessageRecord
    {
        public int Id { get; set; }

        public int OperationId { get; set; }

        public DateTime ProcessDate { get; set; }

        public string DocType { get; set; }

        public string DocNumber { get; set; }

        public string Contact { get; set; }

        public string ContractNumber { get; set; }

        public string Value { get; set; }

        public string MessageBody { get; set; }

        public string MessageType { get; set; }

        public string RecipientType { get; set; }

    }
}
