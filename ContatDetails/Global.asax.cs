using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace ContatDetails
{
    public class MvcApplication : System.Web.HttpApplication, IContainerProviderAccessor
    {
        private static IContainer _container;
        private static IContainerProvider _containerProvider;

        // Instance property that will be used by Autofac HttpModules
        // to resolve and inject dependencies.
        public IContainerProvider ContainerProvider => _containerProvider;

        public object IocConfig { get; private set; }

        protected void Application_Start()
        {
          
            AreaRegistration.RegisterAllAreas();

            var DisplayMode = DisplayModeProvider.Instance.Modes;
            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("WP")
            {
                ContextCondition = (ctx => ctx.GetOverriddenUserAgent()
                .IndexOf("Windows Phone OS", StringComparison.OrdinalIgnoreCase) > 0)

            });
            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("iPhone")
            {
                ContextCondition = (ctx => ctx.GetOverriddenUserAgent()
                .IndexOf("iPhone", StringComparison.OrdinalIgnoreCase) > 0)

            });
            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode("Android")
            {
                ContextCondition = (ctx => ctx.GetOverriddenUserAgent()
                .IndexOf("Android", StringComparison.OrdinalIgnoreCase) > 0)

            });
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
         
            var builder = new ContainerBuilder();
          
            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                       .PropertiesAutowired();
            builder
               .RegisterType<ContactDetailsEntities>()
               .AsSelf()
               .InstancePerRequest();
            builder.RegisterType<ContactDetailsEntities>().As<DbContext>();
  
            _container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));

        }
    }
}
