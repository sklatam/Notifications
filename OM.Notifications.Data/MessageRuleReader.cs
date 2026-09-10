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
    public class MessageRuleReader : Common.Data.ReaderBase<SqlProvider, DataModels.MessageRule>
    {
        public override string ConnectionString => OM.Common.Configuration.Manager.Instance["SkCo.Notificaciones"]["ConnectionString"];

        protected override DbCommand GetCommand()
        {
            return this.Database.GetStoredProcCommand("prc_reglasenvio_select");
        }

        protected override void MapFields(IEntityMapper<MessageRule> mapper)
        {
            mapper
                .Map(r => r.Parameter, "Parametro")
                .Map(r => r.Operator, "Operador")
                .Map(r => r.Value, "Valor");
        }
    }
}
