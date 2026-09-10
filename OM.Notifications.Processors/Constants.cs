using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Notifications.Processors
{
    public static class Constants
    {
        public const int Colombia = 1;
        public const int Mexico = 2;
        public const int MessageBillingRuleEnvio = -7; //No cumple regla de Envio MessageBilling
        public const int NoCumpleReglaEnvio = 7; //No cumple regla de Envio estados skandia
        public const int Error = 4;//Error Generico para estados skandia
        public const int ErrorMessageBilling = -4;//Error Generico para  MessageBilling
        public const int ENVIADOALOPERADOR = 1;//Enviado al operador SMS o EMail()
        public const int NoEnviado = 0; //No NoEnviado
        public const int MessageBillingNoEnviado = 0; //No NoEnviado
    }
}
