using API.Infrastructure;
using API.Interfaces;
using API.Services;
using Autofac;
using Infrastructure;
using Infrastructure.Interfaces;

namespace API
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SourceService>().As<ISourceService>().InstancePerDependency();
            builder.RegisterType<SourceManager>().As<ISourceManager>().InstancePerLifetimeScope();

            builder.RegisterType<TextService>().As<ITextService>().InstancePerDependency();
            builder.RegisterType<TextManager>().As<ITextManager>().InstancePerLifetimeScope();

            builder.RegisterType<EventService>().As<IEventService>().InstancePerDependency();
            builder.RegisterType<EventManager>().As<IEventManager>().InstancePerLifetimeScope();

            builder.RegisterType<FileDataExporter>().As<IFileDataExporter>().InstancePerDependency();
        }
    }
}
