using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace OM.Notifications
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Startup.MappingConfig.Configure();
                var container = Startup.ServicesConfig.Configure();

                var service = new Service(
                    new Unity.Wcf.UnityServiceHost(container, typeof(BLL.Impl.NotificationsService)),
                    container.Resolve<Quartz.IScheduler>());

                if (Environment.UserInteractive)
                {
                    service.Startup();

                    Console.WriteLine("Running. Press any key to stop");
                    Console.ReadKey(true);

                    service.Shutdown();
                }
                else
                {
                    ServiceBase.Run(service);
                }
            }
            catch (Exception ex)
            {
                if (!Environment.UserInteractive)
                    throw;

                Console.Error.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
