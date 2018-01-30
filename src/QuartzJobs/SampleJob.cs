using System;
using System.Threading.Tasks;
using Quartz;

namespace QuartzJobs {

    public class SampleJob : IJob {

        public async Task Execute(IJobExecutionContext context) {
            Console.WriteLine("SampleJob runing!");
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine("SampleJob run finished");
        }

    }

}
