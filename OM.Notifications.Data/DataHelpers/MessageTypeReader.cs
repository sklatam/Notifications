using OM.Common.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using OM.Common.Data;
using OM.Notifications.Data.DataModels;

namespace OM.Notifications.Data.DataHelpers
{
    public class MessageTypeReader : OM.Common.Data.ReaderBase<SqlProvider, DataModels.NotificationType>
    {
        public override string ConnectionString => Constants.Default;

        protected override DbCommand GetCommand()
        {
            var command = this.Database.GetStoredProcCommand("prc_tipomensaje_select_idmensaje");
            return command;
        }

        protected override void SetParameters(DbCommand command, object[] parameters)
        {
            this.Database.AddInParameter(command, "@idOperacion", System.Data.DbType.Int32, parameters[0]);
            this.Database.AddInParameter(command, "@Proveedor", System.Data.DbType.Int32, parameters[1]);
        }

        protected override void MapFields(IEntityMapper<NotificationType> mapper)
        {
            mapper
                .Map(dest => dest.Country, "PAIS")
                .Map(dest => dest.AccountUser, "UsuarioCuenta")
                .Map(dest => dest.AccountKey, "ClaveCuenta")
                .Map(dest => dest.MessageName, "NombreMensaje")
                .Map(dest => dest.Mail, "MAIL");
        }
    }
}
