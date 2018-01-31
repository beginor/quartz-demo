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
            schedulerFactory = new StdSchedulerFactory();

            scheduler = await schedulerFactory.GetScheduler().ConfigureAwait(false);
            await scheduler.Start().ConfigureAwait(false);
        }

    }

}
