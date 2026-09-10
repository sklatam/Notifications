using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace OM.Notifications.BLL.Impl.Builders
{
    public interface IMessageBuilder
    {
        IEnumerable<Processors.Message> BuildMessages(int operationId, string docNumber, string docType, int contractNumber, string productCode, decimal amount, bool sendCopyFp, int operationFpId, Entities.MessageContact[] contacts, Entities.MessageVariable[] variables);
    }
}
