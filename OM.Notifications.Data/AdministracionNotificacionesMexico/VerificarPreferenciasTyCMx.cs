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
    public class VerificarPreferenciasTyCMx : ScalarReaderBase<SqlProvider, int>
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
            var command = this.Database.GetStoredProcCommand("prc_Preferencias_Editor_Transacciones_get_Terminos_y_condiciones_mx");

            this.Database.AddInParameter(command, "IdPais", System.Data.DbType.Int32, parameters[0]);
            this.Database.AddInParameter(command, "User", System.Data.DbType.String, parameters[1]);
            this.Database.AddInParameter(command, "ClienteRFC", System.Data.DbType.String, parameters[2]);
            this.Database.AddInParameter(command, "Movil", System.Data.DbType.String, parameters[3]);
            this.Database.AddInParameter(command, "Contrato", System.Data.DbType.String, parameters[4]);
            this.Database.AddInParameter(command, "TipoTerminos", System.Data.DbType.String, parameters[5]);
            this.Database.AddInParameter(command, "TipoOperacion", System.Data.DbType.Int32, parameters[6]);
            this.Database.AddInParameter(command, "Operacion", System.Data.DbType.String, parameters[7]);

            return command;
        }
    }
}
