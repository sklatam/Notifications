using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OM.Common.Utils.EnumDescription;
using OM.Notifications.Entities;

namespace OM.Notifications.BLL.Impl.Helpers
{
    public static class ContactExtensions
    {
        /// <summary>
        /// Método que retorna el listado de información de contacto
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>Listado de Información de contacto o destino</returns>
        public static MessageContact[] GetContacts(this System.Data.DataRow dr)
        {
            var isOnline = dr.Table.Columns.Contains("EnvioEnLinea") && Convert.ToBoolean(dr["EnvioEnLinea"]);
            return new []
            {
                new MessageContact
                {
                    Recipient = ContactRecipient.Customer,
                    Contact = dr["celularcliente"].ToString(),
                    UnifiedReference = dr["ReferenciaUnica"].ToString(),
                    MessageBody = dr["CuerpoMensajeCliente"].ToString(),
                    FirstName = dr["PERS_FirstName"].ToString(),
                    LastName = dr["PERS_LastName"].ToString(),
                    Type = dr["TipoMensajeCliente"].ToString() == "SMS" ? MessageType.SMS : MessageType.Email,
                    IsOnline = isOnline,
                },
                new MessageContact
                {
                    Recipient = ContactRecipient.Agent,
                    Contact = dr["celularfp"].ToString(),
                    UnifiedReference = dr["ReferenciaUnica"].ToString(),
                    MessageBody = dr["CuerpoMensajeFP"].ToString(),
                    Type = dr["TipoMensajeCliente"].ToString() == "SMS" ? MessageType.SMS : MessageType.Email,
                    IsOnline = isOnline,
                }
            };
        }

        /// <summary>
        /// Método que retorna el listado de información de contacto
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="contacts"></param>
        /// <returns>Listado de Información de contacto o destino</returns>
        public static IEnumerable<MessageContact> GetContacts(this DataRow dr, MessageContact[] contacts)
        {
            if (contacts != null)
            {
                var contactCustomer = contacts.FirstOrDefault(
                    c => c.Recipient.Equals(ContactRecipient.Customer) && c.Type.Equals(EnumDescriptor.GetValue<MessageType>(dr["TipoMensajeCliente"].ToString())));

                if (contactCustomer != null)
                    yield return new MessageContact
                    {
                        Recipient = ContactRecipient.Customer,
                        Contact = contactCustomer.Contact,
                        UnifiedReference = contactCustomer.UnifiedReference,
                        MessageBody = dr["CuerpoMensajeCliente"].ToString(),
                        FirstName = dr.Table.Columns.Contains("PERS_FirstName") ? dr["PERS_FirstName"].ToString() : string.Empty,
                        LastName = dr.Table.Columns.Contains("PERS_LastName") ? dr["PERS_LastName"].ToString() : string.Empty,
                        Type = dr["TipoMensajeCliente"].ToString() == "SMS" ? MessageType.SMS : MessageType.Email,
                        IsOnline = dr.Table.Columns.Contains("EnvioEnLinea") && Convert.ToBoolean(dr["EnvioEnLinea"])
                    };

                var contactAgent = contacts.FirstOrDefault(
                   c => c.Recipient.Equals(ContactRecipient.Agent) && c.Type.Equals(EnumDescriptor.GetValue<MessageType>(dr["TipoMensajeCliente"].ToString())));

                if (contactAgent != null)
                    yield return new MessageContact
                    {
                        Recipient = ContactRecipient.Agent,
                        Contact = contactAgent.Contact,
                        UnifiedReference = contactAgent.UnifiedReference,
                        MessageBody = dr["CuerpoMensajeFP"].ToString(),
                        Type = dr["TipoMensajeCliente"].ToString() == "SMS" ? MessageType.SMS : MessageType.Email,
                        IsOnline = dr.Table.Columns.Contains("EnvioEnLinea") && Convert.ToBoolean(dr["EnvioEnLinea"])
                    };
            }
        }
    }
}
