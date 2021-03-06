﻿using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace QuartzManager.Controllers {

    public class Installer : IWindsorInstaller {

        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(
                Classes.FromThisAssembly()
                    .Where(type => type.Name.EndsWith("Controller", StringComparison.Ordinal))
                    .Configure(cfg => cfg.LifestyleTransient())
            );
        }

    }

}
