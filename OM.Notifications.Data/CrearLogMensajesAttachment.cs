using OM.Common.Data;
using OM.Common.Data.Providers;
using System.Data.Common;

namespace OM.Notifications.Data
{
    public class CrearLogMensajesAttachment : ScalarReaderBase<SqlProvider, string>
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
            var command = this.Database.GetStoredProcCommand("Insert_LogMensajesAttachment");

            this.Database.AddInParameter(command, "IdTemplate", System.Data.DbType.Int32, parameters[0]);
            this.Database.AddInParameter(command, "EmailAdressFrom", System.Data.DbType.String, parameters[1]);
            this.Database.AddInParameter(command, "EmailAdressTo", System.Data.DbType.String, parameters[2]);
            this.Database.AddInParameter(command, "Subject", System.Data.DbType.String, parameters[3]);
            this.Database.AddInParameter(command, "Variables", System.Data.DbType.String, parameters[4]);
            this.Database.AddInParameter(command, "NoOfAttachments", System.Data.DbType.Int32, parameters[5]);
            this.Database.AddInParameter(command, "DocNumber", System.Data.DbType.String, parameters[6]);

            return command;
        }
    }
}
