using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace OM.Notifications
{
    public class Service : ServiceBase
    {
        private readonly ServiceHost _serviceHost;
        private readonly IScheduler _jobScheduler;

        public Service(
            ServiceHost serviceHost,
            IScheduler jobScheduler)
        {
            this._serviceHost = serviceHost;
            this._jobScheduler = jobScheduler;
        }

        internal void Startup()
        {
            this._serviceHost.Open();

            if (Properties.Settings.Default.BackgroundProcessorJobEnabled)
            {
                // Define the job and tie it to our HelloJob class
                var processorJob = JobBuilder.Create<Jobs.ProcessMessageJob>()
                    .Build();

                // Trigger the job to run now, and then repeat every X minutes
                var processorTrigger = TriggerBuilder.Create()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(Properties.Settings.Default.BackgroundProcessorJobIntervalMinutes)
                        .RepeatForever())
                    .StartNow()
                    .Build();

                // Tell quartz to schedule the job using our trigger
                this._jobScheduler.ScheduleJob(processorJob, processorTrigger);
                this._jobScheduler.Start();
            }
        }

        internal void Shutdown()
        {
            this._serviceHost.Close();
            this._jobScheduler.Clear();
        }

        #region ServiceBase Events

        protected override void OnStart(string[] args)
        {
            this.Startup();
        }

        protected override void OnStop()
        {
            this.Shutdown();
        }

        #endregion
    }
}
