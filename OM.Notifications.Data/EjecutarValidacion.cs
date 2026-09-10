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
    public class EjecutarValidacion : OM.Common.Data.DataSetReaderBase<SqlProvider, DataSet>
    {
        private string validacion;

        public EjecutarValidacion(string validacion)
        {
            this.validacion = validacion;
        }

        public override string ConnectionString
        {
            get
            {
                return OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["ConnectionString"];
            }
        }

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            var command = this.Database.GetStoredProcCommand(this.validacion);

            this.Database.AddInParameter(command, "@operationId", System.Data.DbType.Int32, parameters[0]);
            this.Database.AddInParameter(command, "@docNumber", System.Data.DbType.String, parameters[1]);
            this.Database.AddInParameter(command, "@docType", System.Data.DbType.String, parameters[2]);
            this.Database.AddInParameter(command, "@contractNumber", System.Data.DbType.Int32, parameters[3]);
            this.Database.AddInParameter(command, "@productCode", System.Data.DbType.String, parameters[4]);
            this.Database.AddInParameter(command, "@amount", System.Data.DbType.Decimal, parameters[5]);

            return command;
        }
    }
}
