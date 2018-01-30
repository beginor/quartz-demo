using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace QuartzRunner {

    class Program {

        private static ISchedulerFactory schedulerFactory;
        private static IScheduler scheduler;

        static void Main(string[] args) {
            var task = Run();
            task.Wait();
            
            Console.WriteLine("Simple job server runing, press Enter to exit!");
            Console.ReadLine();

            var shutdownTask = scheduler.Shutdown();
            shutdownTask.Wait();
        }

        static async Task Run() {
            var props = new NameValueCollection {
                ["quartz.scheduler.instanceName"] = "ServerScheduler",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.threadPool.threadCount"] = "10",
                ["quartz.serializer.type"] = "JSON",
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.PostgreSQLDelegate, Quartz",
                ["quartz.jobStore.dataSource"] = "myDS",
                ["quartz.dataSource.myDS.connectionString"] = "Server=127.0.0.1;Port=5432;Database=quartz;User Id=postgres;Password=1a2b3c4D;",
                ["quartz.dataSource.myDS.provider"] = "Npgsql",
                ["quartz.jobStore.clustered"] = "true",
                ["quartz.scheduler.instanceId"] = "AUTO",
                //["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz",
                //["quartz.scheduler.exporter.port"] = "5550",
                //["quartz.scheduler.exporter.bindName"] = "QuartzScheduler",
                //["quartz.scheduler.exporter.channelType"] = "tcp",
                //["quartz.scheduler.exporter.channelName"] = "httpQuartz"
            };
            schedulerFactory = new StdSchedulerFactory(props);

            scheduler = await schedulerFactory.GetScheduler().ConfigureAwait(false);
            await scheduler.Start().ConfigureAwait(false);
        }

    }

}
