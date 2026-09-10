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
    public class ComunicacionAutomaticaInsert : OM.Common.Data.RunnerBase<SqlProvider>
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
            var command = this.Database.GetStoredProcCommand("prc_C_E_comunicacion_Automatica_insert");

            this.Database.AddInParameter(command, "Campana", System.Data.DbType.String, parameters[0]);
            this.Database.AddInParameter(command, "Documento", System.Data.DbType.String, parameters[1]);
            this.Database.AddInParameter(command, "Tipo_Doc", System.Data.DbType.String, parameters[2]);
            this.Database.AddInParameter(command, "email", System.Data.DbType.String, parameters[3]);
            this.Database.AddInParameter(command, "campo1_text", System.Data.DbType.String, parameters[4]);

            return command;
        }

    }
}
