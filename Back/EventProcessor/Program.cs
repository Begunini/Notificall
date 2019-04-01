using System;
using System.Configuration;
using Autofac;
using Database;
using EventProcessor.Infrastructure;
using EventProcessor.Interfaces;
using Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using EventHandler = EventProcessor.Infrastructure.EventHandler;

namespace EventProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("AzureWebJobsDashboard", "UseDevelopmentStorage=true");
            Environment.SetEnvironmentVariable("AzureWebJobsStorage", "UseDevelopmentStorage=true");

            Environment.SetEnvironmentVariable("BlobStorage_ConnectionString", ConfigurationManager.AppSettings["BlobStorage_ConnectionString"]);
            Environment.SetEnvironmentVariable("BlobStorage_Container", ConfigurationManager.AppSettings["BlobStorage_Container"]);

            var builder = new ContainerBuilder();

            var connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
            var options = new DbContextOptionsBuilder<NotificallDbContext>().UseSqlServer(connectionString).Options;
            builder.Register(b => new NotificallDbContext(options)).InstancePerDependency();

            var yandexUrl = ConfigurationManager.AppSettings["YandexUrl"];
            var yandexApiKey = ConfigurationManager.AppSettings["YandexApiKey"];
            var defaultLanguage = ConfigurationManager.AppSettings["DefaultLanguage"];
            builder.Register(b =>
                new VoiceService(yandexUrl, yandexApiKey, defaultLanguage, AudioFormat.wav, AudioQuality.hi)).As<IVoiceService>().InstancePerDependency();

            builder.RegisterType<Functions>().InstancePerDependency();

            builder.RegisterType<EventHandler>().As<IEventHandler>().InstancePerDependency();

            builder.RegisterType<SourceManager>().As<ISourceManager>().InstancePerLifetimeScope();
            builder.RegisterType<TextManager>().As<ITextManager>().InstancePerLifetimeScope();
            builder.RegisterType<EventManager>().As<IEventManager>().InstancePerLifetimeScope();
            builder.RegisterType<CallManager>().As<ICallManager>().InstancePerLifetimeScope();

            builder.RegisterType<TelephonyHandler>().As<ITelephonyHandler>().SingleInstance();

            builder.RegisterType<BlobManager>().As<IBlobManager>().InstancePerDependency();


            var config = new JobHostConfiguration
            {
                JobActivator = new AutofacJobActivator(builder.Build())
            };

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }
            config.UseTimers();

            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
