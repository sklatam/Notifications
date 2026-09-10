using OM.Common.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace OM.Notifications.Data
{
    public class ConsultarByMensajeCategoriaMx : OM.Common.Data.DataSetReaderBase<SqlProvider, DataSet>
    {
        public override string ConnectionString => OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["ConnectionString"];

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            var command = this.Database.GetStoredProcCommand("prc_mensajes_select_By_categoria_Mensajes_mx");

            this.Database.AddInParameter(command, "IdOperacion", DbType.Int32, parameters[0]);
            this.Database.AddInParameter(command, "contrato", DbType.String, parameters[1]);
            this.Database.AddInParameter(command, "monto", DbType.String, parameters[2]);

            return command;
        }
    }
}
