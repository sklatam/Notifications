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
    public class ConsultarPreferencias : OM.Common.Data.DataSetReaderBase<SqlProvider, DataSet>
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
            var command = this.Database.GetStoredProcCommand("prc_Preferencias_Editor_Transacciones_get_ArbolCategoriaPais");
            this.Database.AddInParameter(command, "IdPais", System.Data.DbType.Int32, parameters[0]);
            return command;
        }
    }
}
