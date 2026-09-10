using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

namespace OM.Notifications.BLL.Impl.Helpers
{
    public static class SecureAttachmentExtensions
    {
        public static Entities.MessageAttachment SecureContent(this Entities.MessageAttachment attachment)
        {
            if (!string.IsNullOrEmpty(attachment.EncryptionKey))
            {
                byte[] bytes;

                using (var input = new MemoryStream(Convert.FromBase64String(attachment.Content)))
                {
                    using (var output = new MemoryStream())
                    {
                        var reader = new PdfReader(input);
                        var stamper = new PdfStamper(reader, output);

                        stamper.SetEncryption(PdfWriter.ENCRYPTION_AES_256, attachment.EncryptionKey, attachment.EncryptionKey, PdfWriter.ALLOW_SCREENREADERS);
                        stamper.Close();
                        reader.Close();

                        bytes = output.ToArray();
                    }
                }

                attachment.Content = Convert.ToBase64String(bytes);
            }

            return attachment;
        }

        public static Entities.MessageAttachment[] SecureContent(this IEnumerable<Entities.MessageAttachment> attachments)
        {
            return (attachments ?? Enumerable.Empty<Entities.MessageAttachment>())
                .Select(a => a.SecureContent())
                .ToArray();
        }
    }
}
