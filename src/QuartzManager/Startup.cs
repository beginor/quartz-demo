using System.Web.Http;
using System.Web.Http.Cors;
using Beginor.Owin.WebApi.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Owin;
using Quartz;
using Quartz.Impl;
using QuartzManager.Controllers;

namespace QuartzManager {

    public class Startup {

        public void Configuration(IAppBuilder app) {
            // ConfigStaticFile(app);
            //  ConfigOauth(app);
            ConfigWebApi(app);
        }
        
        private static void ConfigWebApi(IAppBuilder app) {
            var config = new HttpConfiguration();
            // remove xml formatter
            var xml = config.Formatters.XmlFormatter;
            config.Formatters.Remove(xml);
            // config json formatter
            var json = config.Formatters.JsonFormatter;
            json.Indent = true;
            json.UseDataContractJsonSerializer = false;
            // enable cors
            ConfigWebApiCors(config);
            // web api routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "rest/{controller}/{id}"
            );

            var container = ConfigWindsor();
            config.UseWindsorContainer(container);
            
            app.UseWebApi(config);
        }
        
        private static void ConfigWebApiCors(HttpConfiguration config) {
            var policy = new EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*"
            ) {
                SupportsCredentials = true
            };
            config.EnableCors(policy);
        }

        private static IWindsorContainer ConfigWindsor() {
            var container = new WindsorContainer();

            container.Register(
                Component.For<IScheduler>()
                    .LifestyleSingleton()
                    .UsingFactoryMethod(
                        () => {
                            var schedulerFactory = new StdSchedulerFactory();
                            var getSchedulerTask =
                                schedulerFactory.GetScheduler();
                            getSchedulerTask.Wait();
                            return getSchedulerTask.Result;
                        }
                    ),
                Component.For<IWindsorContainer>()
                    .LifestyleSingleton()
                    .Instance(container),
                Component.For<JobsController>()
                    .LifestyleTransient()
            );
            return container;
        }

    }

}
