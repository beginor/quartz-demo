using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using QuartzJobs;

namespace QuartzManager {

    class Program {
        
        private static ISchedulerFactory schedulerFactory;
        private static IScheduler scheduler;

        static void Main(string[] args) {
            var task = Run();
            task.Wait();
        }

        static async Task Run() {
            var props = new NameValueCollection {
                ["quartz.scheduler.instanceName"] = "ServerScheduler",
                ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
                ["quartz.threadPool.threadCount"] = "10",
                ["quartz.scheduler.proxy"] = "true",
                ["quartz.scheduler.proxy.address"] = "tcp://127.0.0.1:5550/QuartzScheduler"
            };
            schedulerFactory = new StdSchedulerFactory(props);

            scheduler = await schedulerFactory.GetScheduler();

            var job = JobBuilder.Create<SampleJob>()
                .WithIdentity("RemoteJob", "default")
                .Build();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("RemoteJob", "default")
                .ForJob(job.Key)
                .WithCronSchedule("/5 * * ? * *")
                .Build();

            if (await scheduler.CheckExists(job.Key)) {
                await scheduler.DeleteJob(job.Key);
            }

            await scheduler.ScheduleJob(job, trigger);
            
            Console.WriteLine(scheduler);
        }

    }

}
