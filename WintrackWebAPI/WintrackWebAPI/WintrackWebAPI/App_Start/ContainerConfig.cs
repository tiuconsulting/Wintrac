using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using WintrackBO.Interface;
using WintrackDAO.Interface;
using WintrackDAO;
using WintrackBO;

namespace WintrackWebAPI.App_Start
{
    /// <summary>
    /// This is used for dependency injection.
    /// This is the prefered way when we have a test driven environnement.
    /// used in this project with a future perspective to provide automated test cases on a CI/CD pipeline if decided for a container based deployment
    /// </summary>
    public static class ContainerConfig
    {
        public static IContainer RegisterAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<CommonBO>().As<ICommonBO>();
            builder.RegisterType<CommonDAO>().As<IWintrackDAO>();
            builder.RegisterType<WintrackReportsBO>().As<IWintrackReportsBO>();
            builder.RegisterType<ReportsDAO>().As<IWintrackReportsDAO>();
            var container = builder.Build();

            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
            return container;
        }
    }
}