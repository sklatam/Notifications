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
    public class PreferenciasEditor : ScalarReaderBase<SqlProvider, string>
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
            var command = this.Database.GetStoredProcCommand("Prc_Preferencias_Editor_Clientes");

            this.Database.AddInParameter(command, "PERS_IdNumber", System.Data.DbType.String, parameters[0]);
            this.Database.AddInParameter(command, "PERS_IdType", System.Data.DbType.String, parameters[1]);
            this.Database.AddInParameter(command, "IdMensaje", System.Data.DbType.Boolean, parameters[2]);
            this.Database.AddInParameter(command, "UsuarioCrea", System.Data.DbType.String, parameters[3]);
            this.Database.AddInParameter(command, "UsuarioModifica", System.Data.DbType.String, parameters[4]);
            this.Database.AddInParameter(command, "ContacSMS", System.Data.DbType.Int32, parameters[5]);
            this.Database.AddInParameter(command, "ContacUs", System.Data.DbType.Int32, parameters[6]);
            this.Database.AddInParameter(command, "categoria", System.Data.DbType.String, parameters[7]);
            this.Database.AddInParameter(command, "contrato", System.Data.DbType.String, parameters[8]);
            this.Database.AddInParameter(command, "pais", System.Data.DbType.String, parameters[9]);
            this.Database.AddInParameter(command, "strRespuesta", System.Data.DbType.String, parameters[9]);
            this.Database.AddInParameter(command, "Movil", System.Data.DbType.String, parameters[10]);

            return command;
        }
    }
}
