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
    public class CrearEnvioMasivoMensajesPorSolicitud : ScalarReaderBase<SqlProvider, int>
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
            var command = this.Database.GetStoredProcCommand("prc_enviomasivomensajes_insert");

            this.Database.AddInParameter(command, "TipoIdCliente", System.Data.DbType.String, parameters[0]);
            this.Database.AddInParameter(command, "IdCliente", System.Data.DbType.String, parameters[1]);
            this.Database.AddInParameter(command, "IdOperacion", System.Data.DbType.Int32, parameters[2]);
            this.Database.AddInParameter(command, "CuerpoMensaje", System.Data.DbType.String, parameters[3]);
            this.Database.AddInParameter(command, "Contacto", System.Data.DbType.String, parameters[4]);
            this.Database.AddInParameter(command, "Contrato", System.Data.DbType.String, parameters[5]);
            this.Database.AddInParameter(command, "Valor", System.Data.DbType.Decimal, parameters[6]);
            this.Database.AddInParameter(command, "PorSolicitud", System.Data.DbType.Boolean, parameters[7]);
            this.Database.AddInParameter(command, "Envia_A", System.Data.DbType.String, parameters[8]);
            this.Database.AddInParameter(command, "Tipomsj", System.Data.DbType.String, parameters[9]);

            return command;
        }
    }
}
