using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OM.Notifications.Data.DataModels
{
    public class NotificationType
    {
        public int Country { get; set; }

        public string AccountUser { get; set; }

        public string AccountKey { get; set; }

        public string MessageName { get; set; }

        public string Mail { get; set; }
    }
}
