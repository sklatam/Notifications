using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OM.Notifications.Entities
{
    public class MessageAttachment
    {
        public string Content { get; set; }

        public string EncryptionKey { get; set; }

        public string Name { get; set; }

        public string MediaType { get; set; }
    }
}
