using System.Reflection;
using Autofac;
using Czeum.Application.Services;
using Module = Autofac.Module;

namespace Czeum.Api.AutofacModules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(Assembly.Load("Czeum.Application"))
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BoardLoader<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<ServiceContainer>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<MatchConverter>()
                .AsImplementedInterfaces()
                .InstancePerDependency();
        }
    }
}