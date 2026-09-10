using OM.Common.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace OM.Notifications.Data.DataHelpers
{
    public class CategoryReader : OM.Common.Data.ScalarReaderBase<SqlProvider, int>
    {
        public override string ConnectionString => Constants.Default;

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            var command = this.Database.GetStoredProcCommand("prc_Buscar_Operacion");

            this.Database.AddInParameter(command, "Operacion", System.Data.DbType.Int32, parameters[0]); 
            this.Database.AddInParameter(command, "IsFromNewService", System.Data.DbType.Boolean, true);

            return command;
        }
    }
}
