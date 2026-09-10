using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OM.Notifications.Processors.BizTalkProxy.Entities
{
    public partial class NotificacionesRequest
    {
        public void AddParameter(string name, string value)
        {
            var @params = new List<NotificacionesRequestParameters>(this.Parameters ?? Enumerable.Empty<NotificacionesRequestParameters>());

            @params.Add(
                new NotificacionesRequestParameters
                {
                    Name = name,
                    Value = value,
                });

            this.Parameters = @params.ToArray();
        }

        public string GetParameter(string name)
        {
            return (this.Parameters ?? Enumerable.Empty<NotificacionesRequestParameters>())
                .Where(p => p.Name == name)
                .Select(p => p.Value)
                .FirstOrDefault();
        }
    }
}
