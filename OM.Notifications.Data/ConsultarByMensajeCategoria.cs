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
    public class ConsultarByMensajeCategoria : OM.Common.Data.DataSetReaderBase<SqlProvider, DataSet>
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
            var command = this.Database.GetStoredProcCommand("prc_mensajes_select_By_categoria_Mensajes");

            this.Database.AddInParameter(command, "IdOperacion", System.Data.DbType.Int32, parameters[0]);
            this.Database.AddInParameter(command, "contrato", System.Data.DbType.String, parameters[1]);
            this.Database.AddInParameter(command, "NumIdCliente", System.Data.DbType.String, parameters[2]);
            this.Database.AddInParameter(command, "producto", System.Data.DbType.String, parameters[3]);
            this.Database.AddInParameter(command, "monto", System.Data.DbType.String, parameters[4]);
            if(parameters[5] != null) 
                this.Database.AddInParameter(command, "fecha", System.Data.DbType.String, parameters[5]);
            return command;
        }
    }
}
