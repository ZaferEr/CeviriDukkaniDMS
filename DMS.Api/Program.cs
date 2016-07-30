using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.WebApi;
using DMS.Business;
using DMS.Business.ExternalServices;
using DMS.Business.Services;
using log4net;
using MassTransit;
using Microsoft.Owin.Hosting;
using RabbitMQ.Client;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Mappers;
using Tangent.CeviriDukkani.Event.DocumentEvents;
using Tangent.CeviriDukkani.Logging;
using Tangent.CeviriDukkani.Messaging;
using Tangent.CeviriDukkani.Messaging.Consumer;
using Tangent.CeviriDukkani.Messaging.Producer;

namespace DMS.Api {
    public class Program {
        static void Main(string[] args) {
            string baseAddress = "http://localhost:8003/";

            Bootstrapper();
            Console.WriteLine("Bootstrapper finished");

            var webApp = WebApp.Start<Startup>(url: baseAddress);
            Console.WriteLine($"DMS is ready in {baseAddress}");

            Container.Resolve<DmsEventProjection>().Start();
            Console.WriteLine("Projection started...");
            Console.ReadLine();

            Console.WriteLine("Starting to close DMS...");

            CustomLogger.Logger.Info($"DMS service is down with projections {DateTime.Today}");

            //Container.Resolve<IConnection>().Close();
        }

        public static void Bootstrapper() {
            var builder = new ContainerBuilder();
            builder.RegisterCommons();
            builder.RegisterBusiness();

            var settings = builder.RegisterSettings();
            builder.RegisterEvents(settings);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<DmsEventProjection>().AsSelf().SingleInstance();

            Container = builder.Build();
            CustomLogger.Logger.Info($"DMS service is up and ready with projections {DateTime.Today}");
        }

        public static IContainer Container { get; set; }
    }

    public static class AutofacExtensions {
        public static void RegisterCommons(this ContainerBuilder builder) {

            builder.RegisterType<CeviriDukkaniModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomMapperConfiguration>().AsSelf().SingleInstance();
            builder.RegisterInstance(CustomLogger.Logger).As<ILog>().SingleInstance();
        }

        public static void RegisterBusiness(this ContainerBuilder builder) {
            builder.RegisterType<DocumentService>().As<IDocumentService>().InstancePerLifetimeScope();
            builder.RegisterType<TranslationService>().As<ITranslationService>().InstancePerLifetimeScope();
        }

        public static void RegisterEvents(this ContainerBuilder builder, Settings settings) {
            var connection = new RabbitMqConnectionFactory(settings.RabbitHost, settings.RabbitPort, settings.RabbitUserName, settings.RabbitPassword).CreateConnection();
            var dispatcher = new RabbitMqDispatcherFactory(connection, settings.RabbitExchangeName, CustomLogger.Logger).CreateDispatcher();

            builder.RegisterInstance<IConnection>(connection);
            builder.RegisterInstance<IDispatchCommits>(dispatcher);
        }

        public static Settings RegisterSettings(this ContainerBuilder builder) {
            var settings = new Settings {
                RabbitExchangeName = ConfigurationManager.AppSettings["RabbitExchangeName"],
                RabbitHost = ConfigurationManager.AppSettings["RabbitHost"],
                RabbitPassword = ConfigurationManager.AppSettings["RabbitPassword"],
                RabbitPort = int.Parse(ConfigurationManager.AppSettings["RabbitPort"]),
                RabbitUserName = ConfigurationManager.AppSettings["RabbitUserName"]
            };

            builder.RegisterInstance(settings);
            return settings;
        }
    }
}
