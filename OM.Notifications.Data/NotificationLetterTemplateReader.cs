using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OM.Common.Data;
using OM.Common.Data.Providers;
using System.Data;
using System.Data.Common;

namespace OM.Notifications.Data
{
    public class NotificationLetterTemplateReader : OM.Common.Data.ScalarReaderBase<SqlProvider, int?>
    {
        public override string ConnectionString => OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["ConnectionString"];

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            var command = this.Database.GetStoredProcCommand("Get_EmailTemplateForNotification");

            this.Database.AddInParameter(command, "operationId", System.Data.DbType.Int32, parameters[0]);

            return command;
        }

        protected override int? ConvertValue(object value)
        {
            return value != null ?
                Convert.ToInt32(value)
                : (int?)null;
        }
    }
}
