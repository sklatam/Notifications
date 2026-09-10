using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper;
using NLog;
using Quartz;
using Newtonsoft.Json;

namespace OM.Notifications.Jobs
{
    public class ProcessMessageJob : IJob
    {
        private readonly Processors.IMessageProcessor _messageProcessor;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        // Miembros estáticos para mantener solamente una instancia del proceso
        private static bool IsProcessing = false;

        public ProcessMessageJob(
            Processors.IMessageProcessor messageProcessor)
        {
            this._messageProcessor = messageProcessor;
        }

        /// <summary>
        ///  Este job corresponde a la migración del puerto SQL de la solucino en BizTalk,
        ///  el cual realiza un polling a la BD para obtener mensajes pendientes y los envía a la orquestacion de procesamiento 
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            // Quartz dispara una instancia del proceso cada X minutos
            // Si al lanzar una instancia se detecta que ya hay otra en ejecución, no se hace nada.
            if (ProcessMessageJob.IsProcessing)
            {
                this._logger.Info("Ya existe una instancia del proceso activa.");
                return;
            }

            try
            {
                ProcessMessageJob.SetProcessing(true);
                this._logger.Info("Executing Processor Job");

                // 1. Obtener mensajes pendientes desde la BD
                var pendingMessages = new Data.PendingMessageReader()
                    .Get();
                var pendingUpdater = new Data.PendingMessageUpdater();

                foreach (var message in pendingMessages)
                {
                    try
                    {
                        this._logger.Info($"Execute Jobs message: {JsonConvert.SerializeObject(message)}");

                        // 2. Mapear el objeto retornado por el SP al esquema de NotificacionesRequest
                        var messageRequest = Mapper.Map<Data.DataModels.MessageRecord, Processors.Message>(message);

                        // 3. Reprocesar el mensaje
                        this._messageProcessor.ProcessMessage(messageRequest);
                    }
                    catch (Exception ex)
                    {
                        this._logger.Error($"Error reprocesando mensaje pendiente: {ex}");

                        try
                        {
                            pendingUpdater.Run(message.Id);
                        }
                        catch(Exception ex2)
                        {
                            this._logger.Fatal($"Error marcando mensaje pendiente: {ex2}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Quartz jobs should not throw exceptions
                this._logger.Error($"Error en Job: {ex}");
            }
            finally
            {
                ProcessMessageJob.SetProcessing(false);
                this._logger.Info("Finished Processor Job");
            }
        }

        private static void SetProcessing(bool isProcesing)
        {
            ProcessMessageJob.IsProcessing = isProcesing;
        }
    }
}
