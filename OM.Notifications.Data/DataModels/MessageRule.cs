using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OM.Notifications.Data.DataModels
{
    public class MessageRule
    {
        public int OperationId { get; set; }

        public string Parameter { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }
    }
}
