using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace OM.Notifications.Processors.Helpers
{
    public static class MediaTypeExtensions
    {
        public static string AsExtension(this string mediaType)
        {
            switch (mediaType)
            {
                case MediaTypeNames.Text.Html:
                case MediaTypeNames.Text.Xml:
                case MediaTypeNames.Image.Gif:
                case MediaTypeNames.Image.Jpeg:
                case MediaTypeNames.Image.Tiff:
                case MediaTypeNames.Application.Pdf:
                case MediaTypeNames.Application.Rtf:
                case MediaTypeNames.Application.Zip:
                    return mediaType.Split('/')[1];

                case MediaTypeNames.Text.Plain:
                    return "txt";

                case MediaTypeNames.Text.RichText:
                    return "rtf";
                    
                case MediaTypeNames.Application.Soap:
                    return "xml";

                default:
                    return "dat";
            }
        }
    }
}
