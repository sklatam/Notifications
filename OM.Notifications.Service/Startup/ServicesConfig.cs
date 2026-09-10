using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Quartz.Unity;

namespace OM.Notifications.Startup
{
    public static class ServicesConfig
    {
        internal static IUnityContainer Configure()
        {
            var container = new UnityContainer();
            
            // Helpers migrados de logica legacy
            container.RegisterType<Processors.DataHelpers.MessageRuleHelper>();
            container.RegisterType<Processors.DataHelpers.MessageStatusHelper>();
            container.RegisterType<Processors.OnlineService.Impl.MessageSenders.SenderFactory>();

            // Settings para el processor en linea
            container.RegisterType<Processors.OnlineService.Settings.IOnlineProcessorSettings, Properties.Settings>();

            // Online processor
            container.RegisterType<Processors.IMessageProcessor, Processors.OnlineService.Impl.OnlineMessageProcessor>();
            container.RegisterType<Processors.Factory.IMessageProcessorFactory, Processors.OnlineService.WindowsServiceMessageProcessorFactory>();
            
            // Servicio default de envío de Emails
            container.RegisterType<Processors.Services.IMailService, Processors.Services.Impl.SmtpMailService>();

            // Implementacion del servicio utilizada por el endpoint WCF
            container.RegisterType<BLL.INotificationsService, BLL.Impl.NotificationsService>();

            // Quartz extensions
            container.AddNewExtension<QuartzUnityExtension>();

            return container;
        }
    }
}
