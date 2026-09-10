using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper;
using OM.Common.Utils.EnumDescription;

namespace OM.Notifications.Facade.Mapping
{
    public static class MappingConfig
    {
        private static bool IsConfigured = false;

        private static object SyncRoot = new object();

        public static void Configure()
        {
            lock (SyncRoot)
            {
                if (!IsConfigured)
                {
                    Mapper.Reset();

                    Mapper.Register<Processors.Message, Processors.BizTalkProxy.Entities.NotificacionesRequest>()
                        .Member(t => t.IdTablaMasivo, s => s.Id)
                        .Member(t => t.Operacion, s => s.OperationId)
                        .Member(t => t.TipoIdCliente, s => s.DocType)
                        .Member(t => t.NumIdCliente, s => s.DocNumber)
                        .Member(t => t.Contrato, s => s.ContractNumber)
                        .Member(t => t.Producto, s => s.ProductCode)
                        .Member(t => t.Contacto, s => s.Contact)
                        .Member(t => t.FirstName, s => s.FirstName)
                        .Member(t => t.LastName, s => s.LastName)
                        .Member(t => t.Image, s => s.Body)
                        .Member(t => t.EnviaA, s => EnumDescriptor.GetDescription(s.Recipient))
                        .Member(t => t.Tipomsj, s => EnumDescriptor.GetDescription(s.Type))
                        .Value(t => t.IsMasive, false);

                    Mapper.Compile();

                    IsConfigured = true;
                }
            }
        }
    }
}
