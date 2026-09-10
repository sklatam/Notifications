using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using OM.Common.Data;
using OM.Common.Data.Providers;
using OM.Notifications.Data.DataModels;

namespace OM.Notifications.Data
{
    public class PendingMessageReader : Common.Data.ReaderBase<SqlProvider, DataModels.MessageRecord>
    {
        public override string ConnectionString => DataHelpers.Constants.Default;

        protected override DbCommand GetCommand()
        {
            return this.Database.GetStoredProcCommand("prc_enviomasivo_getpendientes");
        }

        protected override void SetParameters(DbCommand command, object[] parameters)
        {
            this.Database.AddInParameter(command, "IsFromNewService", System.Data.DbType.Boolean, true);
            // El parametro se envía fijo TRUE ya que se requiere pra esta implementacion del servicio
        }

        protected override void MapFields(IEntityMapper<MessageRecord> mapper)
        {
            mapper
                .Map(r => r.Id, "IdTablaMasivo")
                .Map(r => r.OperationId, "Operacion")
                .Map(r => r.ProcessDate, "FechaProceso")
                .Map(r => r.DocType, "TipoIdCliente")
                .Map(r => r.DocNumber, "IdCliente")
                .Map(r => r.Contact, "Contacto")
                .Map(r => r.ContractNumber, "Contrato")
                .Map(r => r.Value, "Valor")
                .Map(r => r.MessageBody, "Image")
                .Map(r => r.MessageType, "TipoMsj")
                .Map(r => r.RecipientType, "EnviaA");
        }
    }
}
