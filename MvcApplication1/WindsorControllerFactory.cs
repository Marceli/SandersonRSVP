using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Castle.Core;
using Castle.Core.Resource;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;

namespace WebUI
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private WindsorContainer container;

        public WindsorControllerFactory()
        {
            container = new WindsorContainer(new XmlInterpreter(new ConfigResource("castle")));
            var controllertypes =
                Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof (IController).IsAssignableFrom(t));
            foreach (Type type in controllertypes)
            {

                container.AddComponentLifeStyle(type.FullName, type, LifestyleType.Transient);
            }
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext,
            Type controllerType)
        {
            return (IController) container.Resolve(controllerType);
        }
    }
}