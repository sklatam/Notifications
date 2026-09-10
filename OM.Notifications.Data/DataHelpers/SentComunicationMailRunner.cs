using OM.Common.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace OM.Notifications.Data.DataHelpers
{
    public class SentComunicationMailRunner : OM.Common.Data.RunnerBase<SqlProvider>
    {
        public override string ConnectionString => Constants.Default;

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            var command = this.Database.GetStoredProcCommand("prc_EnvioNotificacionMail_db_Comunicaciones_insert");
            Database.AddInParameter(command, "IdTablaEnvioMasivo", System.Data.DbType.Int64, parameters[0]);
            Database.AddInParameter(command, "IdOperacion", System.Data.DbType.Int32, parameters[1]);
            Database.AddInParameter(command, "Documento", System.Data.DbType.String, parameters[2]);
            Database.AddInParameter(command, "TipoDocumento", System.Data.DbType.String, parameters[3]);

            return command;
        }
    }
}
