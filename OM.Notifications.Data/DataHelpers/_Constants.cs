using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OM.Notifications.Data.DataHelpers
{
    internal static class Constants
    {
        internal static string Default => OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["ConnectionString"];

        internal static int PendingMessageStatus => 0;
    }
}
