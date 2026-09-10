using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using NLog;
using OM.Notifications.Data.DataHelpers;

namespace OM.Notifications.Processors.DataHelpers
{
    public class MessageRuleHelper
    {
        // Utilidades del logger
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
       
        /// <summary>
        /// Valida si existe 
        /// canal sms y/o email
        /// preferencias sms y/o email  
        /// numero de celular valido
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public bool CheckRuleOnline(long messageId)
        {
            this._logger.Info($":RuleOnline, Se invoca el metodo de chequeo(Canales,Horario Plantilla,Reintentos,Vigencia y Número de celular valido) Id = {messageId}");
            // Validamos preferencias
            return new ValidateRulesReader().Get(messageId);
        }

        /// <summary>
        /// Valida si el mensaje esta vigente .
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public bool CheckTransactionWithin24Hours(long messageId)
        {
            this._logger.Info($"CheckProcesoEstaDentrolas24Transaccion, Se invoca el metodo de validacion de vigencia del mensaje por solicitud Id = {messageId}");

            //Validamos preferencias
            var validate = new CheckTransactionsReader().Get(messageId);

            this._logger.Info($"CheckProcesoEstaDentrolas24Transaccion, Validation Transaction : {validate}");
            return validate != 0;
        }
        
        /// <summary>
        /// Valida si la plantilla esta en horario de envio
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public bool CheckSchedule(long messageId)
        {
            this._logger.Info($":CheckHorarioEnvioPlantilla, Se invoca el metodo de chequeo Nrodereintento Id={messageId}");

            //Validamos preferencias
            var idMessage = new CheckSentTemplateReader().Get(messageId); 
            return idMessage != 0;
        }

        /// <summary>
        /// Metodo usado por mexico para validar si la cotegoria existe o no
        /// </summary>
        /// <param name="operationId"></param>
        /// <returns></returns>
        public bool GetOperation(int operationId)
        {
            var idCategory = new CategoryReader().Get(operationId);
            return (idCategory > 0);
        }
    }
}

