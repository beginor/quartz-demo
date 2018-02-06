using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Quartz;
using Quartz.Impl.Matchers;

namespace QuartzManager.Controllers {

    [RoutePrefix("rest/jobs")]
    public class JobsController : ApiController {

        private IScheduler scheduler;

        public JobsController(IScheduler scheduler) {
            this.scheduler = scheduler;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                scheduler = null;
            }
            base.Dispose(disposing);
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll() {
            var keys = await scheduler.GetJobKeys(
                GroupMatcher<JobKey>.AnyGroup()
            );
            return Ok(keys);
        }

    }

}
