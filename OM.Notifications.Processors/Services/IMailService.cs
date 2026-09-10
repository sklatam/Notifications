using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OM.Notifications.Entities;

namespace OM.Notifications.Processors.Services
{
    public interface IMailService
    {
        void SendEmail(string from, string to, string subject, string body, MessageVariable[] variables, MessageAttachment[] attachments, MessageContact[] contactsBcc = null);
    }
}
