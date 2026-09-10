using OM.Common.Configuration;
using OM.Notifications.Entities;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using OM.Notifications.Processors.Helpers;

namespace OM.Notifications.Processors.Services.Impl
{
    public class SmtpMailService : IMailService
    {
        private readonly string _emailUserName = Manager.Instance["SkCo.Notificaciones"]["emailUserName"];
        private readonly string _emailPassword = Manager.Instance["SkCo.Notificaciones"]["emailPassword"];
        private readonly string _emailHost = Manager.Instance["SkCo.Notificaciones"]["emailHost"];
        private readonly int _emailPort = int.Parse(Manager.Instance["SkCo.Notificaciones"]["emailPort"]);
        private readonly string _namePadding = "Valor:";

        public void SendEmail(string from, string to, string subject, string body, MessageVariable[] variables, MessageAttachment[] attachments, MessageContact[] contactsBcc = null)
        {
            if (variables?.Any() == true)
            {
                foreach (var variable in variables)
                {
                    var name = variable.Name.Replace("@", string.Empty);
                    // Se ajusta padding para email en linea del PIN
                    var value = variable.Value;
                    int width = 0;
                    Int32.TryParse(body.Substring(body.IndexOf(_namePadding) + _namePadding.Length, 1), out width);
                    if ("Valor".Equals(name))
                        body = body.Replace($"[@{_namePadding}{width}]", value.PadLeft(width, '0'));
                    // Notification email templates
                    body = body.Replace($"[@{name}]", value);
                    // Campaign Enterprise templates
                    body = body.Replace($"{{{name}}}", value);
                }
            }           

            var emailFrom = new MailAddress(from);
            var mailSender = new SmtpClient(_emailHost, _emailPort)
            {
                Credentials = new System.Net.NetworkCredential(_emailUserName, _emailPassword),
            };

            var emailTo = new MailAddress(to);

            var email = new MailMessage(emailFrom, emailTo)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            if (contactsBcc?.Any() == true)
            {
                // Se agregan copias si existen en el contexto
                foreach (var contact in contactsBcc)
                {
                    // Se valida si es un contacto tipo mail no nulo o vacio
                    if (!string.IsNullOrWhiteSpace(contact.Contact) && contact.Contact.Contains("@"))
                    {
                        var emailBcc = new MailAddress(contact.Contact);
                        email.Bcc.Add(emailBcc);
                    }
                }
            }                       

            int count = 1;
            if (attachments?.Any() == true)
            {
                foreach (var item in attachments)
                {
                    var file = new MemoryStream(Convert.FromBase64String(item.Content));
                    file.Seek(0, SeekOrigin.Begin);

                    var mediaType = string.IsNullOrEmpty(item.MediaType) ?
                        System.Net.Mime.MediaTypeNames.Application.Pdf
                        : item.MediaType;

                    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType()
                    {
                        MediaType = mediaType,
                        Name = string.IsNullOrEmpty(item.Name) ? $"Contenido_{count}.{mediaType.AsExtension()}" : item.Name,
                    };

                    var attach = new Attachment(file, ct);
                    email.Attachments.Add(attach);

                    count++;
                }
            }

            mailSender.Send(email);
        }
    }
}
