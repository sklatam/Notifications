using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OM.Common.Data;
using OM.Common.Data.Providers;
using System.Data;
using System.Data.Common;
using System;

namespace OM.Notifications.Data.AdministracionNotificacionesMexico
{
    public class EnqueueEmailMexicoInsert : RunnerBase<SqlProvider>
    {
        public override string ConnectionString
        {
            get
            {
                return OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["ConnectionString"];
            }
        }

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            var command = this.Database.GetStoredProcCommand("prc_EnqueueEmail_Mexico_insert");

            this.Database.AddInParameter(command, "IdTabla", System.Data.DbType.Int32, parameters[0]);
            this.Database.AddInParameter(command, "Value", System.Data.DbType.String, parameters[1]);
            return command;
        }
    }
}
