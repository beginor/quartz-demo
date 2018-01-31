using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
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
            schedulerFactory = new StdSchedulerFactory();
            scheduler = await schedulerFactory.GetScheduler();
            var job = JobBuilder.Create<SampleJob>()
                .WithIdentity("RemoteJob", "default")
                .WithDescription("Remote Job Test")
                .Build();

            if (await scheduler.CheckExists(job.Key)) {
                await scheduler.DeleteJob(job.Key);
            }

            var trigger = TriggerBuilder.Create()
                .WithIdentity("RemoteJob", "default")
                .WithDescription("Remote Job Test")
                .ForJob(job.Key)
                .WithCronSchedule("/5 * * ? * *")
                .Build();
            await scheduler.ScheduleJob(job, trigger);
            
            Console.WriteLine(scheduler);
        }

    }

}
