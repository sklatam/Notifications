using OM.Common.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace OM.Notifications.Data.DataHelpers
{
    public class MarkSentRunner : OM.Common.Data.RunnerBase<SqlProvider>
    {
        public override string ConnectionString => Constants.Default;

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            var command = this.Database.GetStoredProcCommand("prc_enviomasivo_marcarenviado");
            this.Database.AddInParameter(command, "IdTablaEnvioMasivo", System.Data.DbType.Int64, parameters[0]);
            this.Database.AddInParameter(command, "Estado", System.Data.DbType.Int64, parameters[1]);
            this.Database.AddInParameter(command, "MessageBilling", System.Data.DbType.Int64, parameters[2]);

            return command;
        }
    }
}
