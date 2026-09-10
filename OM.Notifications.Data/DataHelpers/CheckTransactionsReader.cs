using OM.Common.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace OM.Notifications.Data.DataHelpers
{
    public class CheckTransactionsReader : OM.Common.Data.ScalarReaderBase<SqlProvider, int>
    {
        public override string ConnectionString => Constants.Default;

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prc_Proceso_Esta_Dentro_las_24_Transaccion");
            this.Database.AddInParameter(command, "Id", System.Data.DbType.Int64, parameters[0]);
            this.Database.AddInParameter(command, "IsFromNewService", System.Data.DbType.Boolean, true);

            return command;
        }
    }
}
