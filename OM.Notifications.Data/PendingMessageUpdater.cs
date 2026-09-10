using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using OM.Common.Data;
using OM.Common.Data.Providers;
using OM.Notifications.Data.DataModels;

namespace OM.Notifications.Data
{
    public class PendingMessageUpdater : Common.Data.RunnerBase<SqlProvider>
    {
        public override string ConnectionString => DataHelpers.Constants.Default;

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            var cmd = this.Database.GetStoredProcCommand("prc_marcarenviado_Proveedor");

            this.Database.AddInParameter(cmd, "IdTablaEnvioMasivo", System.Data.DbType.Int32, parameters[0]);
            this.Database.AddInParameter(cmd, "MessageBilling", System.Data.DbType.Int32, 0);
            this.Database.AddInParameter(cmd, "Token", System.Data.DbType.String, null);
            this.Database.AddInParameter(cmd, "Estado", System.Data.DbType.Int32, DataHelpers.Constants.PendingMessageStatus);

            return cmd;
        }
    }
}
