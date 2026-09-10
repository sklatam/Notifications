using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OM.Common.Data;
using OM.Common.Data.Providers;
using System.Data;
using System.Data.Common;

namespace OM.Notifications.Data.AdministracionNotificacionesMexico
{
    public class UpdatePreferenciasPipeline : OM.Common.Data.RunnerBase<SqlProvider>
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
            var command = this.Database.GetStoredProcCommand("prc_UpdatePreferenciasPipeline");

            this.Database.AddInParameter(command, "operacion", System.Data.DbType.Int32, parameters[0]);
            this.Database.AddInParameter(command, "tipoidCliente", System.Data.DbType.String, parameters[1]);
            this.Database.AddInParameter(command, "IidCliente", System.Data.DbType.String, parameters[2]);
            this.Database.AddInParameter(command, "sms", System.Data.DbType.Boolean, parameters[3]);
            this.Database.AddInParameter(command, "mail", System.Data.DbType.Boolean, parameters[4]);
            this.Database.AddInParameter(command, "user", System.Data.DbType.String, parameters[5]);
            this.Database.AddInParameter(command, "NoSeleccionoRecibirViaSms", System.Data.DbType.Boolean, parameters[6]);
            this.Database.AddInParameter(command, "NoSeleccionoRecibirViaEmail", System.Data.DbType.Boolean, parameters[7]);

            return command;
        }
    }
}
