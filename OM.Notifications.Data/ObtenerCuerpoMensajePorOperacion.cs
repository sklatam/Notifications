using OM.Common.Data;
using OM.Common.Data.Providers;
using System.Data;
using System.Data.Common;

namespace OM.Notifications.Data
{
    public class ObtenerCuerpoMensajePorOperacion : ScalarReaderBase<SqlProvider, string>
    {
        public override string ConnectionString => OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["ConnectionString"];

        protected override DbCommand GetCommandWithParams(object[] parameters)
        {
            var command = this.Database.GetStoredProcCommand("Parameters..NOTIFICATIONLETTER_GetBody");
            this.Database.AddInParameter(command, "ID", DbType.Int32, parameters[0]);           

            return command;
        }
    }
}
