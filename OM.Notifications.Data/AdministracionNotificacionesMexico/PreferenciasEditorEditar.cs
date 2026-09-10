using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OM.Common.Data;
using OM.Common.Data.Providers;
using System.Data;
using System.Data.Common;
using OM.Notifications.Entities;

namespace OM.Notifications.Data.AdministracionNotificacionesMexico
{
    public class PreferenciasEditorEditarRun : RunnerBase<SqlProvider> 
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
            var command = this.Database.GetStoredProcCommand("Prc_Preferencias_Editor_Clientes_Editar");

            this.Database.AddInParameter(command, "PERS_IdNumber", System.Data.DbType.String, parameters[0]);
            this.Database.AddInParameter(command, "ContacUs", System.Data.DbType.Int32, parameters[1]);
            this.Database.AddInParameter(command, "ContacSMS", System.Data.DbType.Int32, parameters[2]);
            this.Database.AddInParameter(command, "Categoria", System.Data.DbType.String, parameters[3]);

            return command;
        }

    }

    public class PreferenciasEditorEditarGet : ScalarReaderBase<SqlProvider,string>
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
            var command = this.Database.GetStoredProcCommand("Prc_Preferencias_Editor_Clientes_Editar");
            
            this.Database.AddInParameter(command, "PERS_IdNumber", System.Data.DbType.String, parameters[0]);
            this.Database.AddInParameter(command, "ContacSMS", System.Data.DbType.Int32, parameters[1]);
            this.Database.AddInParameter(command, "ContacUs", System.Data.DbType.Int32, parameters[2]);
            this.Database.AddInParameter(command, "Categoria", System.Data.DbType.String, parameters[3]);

            return command;
        }

    }

}
