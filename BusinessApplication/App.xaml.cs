using Autofac;
using BusinessApplication.Model;
using BusinessApplication.Repository;
using BusinessApplication.Utility;
using System.Windows;

namespace BusinessApplication
{
    public partial class App : Application
    {
        private static IContainer _container;

        public App()
        {
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Logger>()
                    .WithParameter("initialLoggingServices", new List<ILoggingService> { new PopupLoggingService() })
                    .As<ILogger>()
                    .SingleInstance();

            builder.Register((c) => AppDbContextFactory.Create());

            builder.RegisterType<Repository<Customer>>()
                   .As<IRepository<Customer>>()
                   .InstancePerDependency();

            builder.RegisterType<Repository<Address>>()
                   .As<IRepository<Address>>()
                   .InstancePerDependency();

            builder.RegisterType<Repository<Article>>()
                   .As<IRepository<Article>>()
                   .InstancePerDependency();

            _container = builder.Build();
        }

        public static IContainer Container => _container;
    }
}
