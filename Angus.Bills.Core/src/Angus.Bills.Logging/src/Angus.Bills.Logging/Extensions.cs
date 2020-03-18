using System;
using System.Collections.Generic;
using System.Linq;
using Angus.Bills.Initializers;
using Angus.Bills.Logging.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace Angus.Bills.Logging
{
    public static class Extensions
    {
        private const string LoggerSectionName = "logger";
        private const string AppSectionName = "app";

        public static IHostBuilder UseLogging(this IHostBuilder hostBuilder,
            Action<LoggerConfiguration> configure = null, string loggerSectionName = LoggerSectionName,
            string appSectionName = AppSectionName)
        {
            return hostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                if (string.IsNullOrWhiteSpace(loggerSectionName)) loggerSectionName = LoggerSectionName;

                if (string.IsNullOrWhiteSpace(appSectionName)) appSectionName = AppSectionName;

                var loggerOptions = context.Configuration.GetOptions<LoggerOptions>(loggerSectionName);
                var appOptions = context.Configuration.GetOptions<AppOptions>(appSectionName);

                MapOptions(loggerOptions, appOptions, loggerConfiguration, context.HostingEnvironment.EnvironmentName);
                configure?.Invoke(loggerConfiguration);
            });
        }

        public static IWebHostBuilder UseLogging(this IWebHostBuilder webHostBuilder,
            Action<LoggerConfiguration> configure = null, string loggerSectionName = LoggerSectionName,
            string appSectionName = AppSectionName)
        {
            return webHostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                if (string.IsNullOrWhiteSpace(loggerSectionName)) loggerSectionName = LoggerSectionName;

                if (string.IsNullOrWhiteSpace(appSectionName)) appSectionName = AppSectionName;

                var loggerOptions = context.Configuration.GetOptions<LoggerOptions>(loggerSectionName);
                var appOptions = context.Configuration.GetOptions<AppOptions>(appSectionName);

                MapOptions(loggerOptions, appOptions, loggerConfiguration, context.HostingEnvironment.EnvironmentName);
                configure?.Invoke(loggerConfiguration);
            });
        }

        private static void MapOptions(LoggerOptions loggerOptions, AppOptions appOptions,
            LoggerConfiguration loggerConfiguration, string environmentName)
        {
            if (!Enum.TryParse<LogEventLevel>(loggerOptions.Level, true, out var level))
                level = LogEventLevel.Information;

            loggerConfiguration.Enrich.FromLogContext()
                .MinimumLevel.Is(level)
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", appOptions.Service)
                .Enrich.WithProperty("Instance", appOptions.Instance)
                .Enrich.WithProperty("Version", appOptions.Version);

            foreach (var (key, value) in loggerOptions.Tags ?? new Dictionary<string, object>())
                loggerConfiguration.Enrich.WithProperty(key, value);

            loggerOptions.ExcludePaths?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p))));

            loggerOptions.ExcludeProperties?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty(p)));

            Configure(loggerConfiguration, level, loggerOptions);
        }

        private static void Configure(LoggerConfiguration loggerConfiguration, LogEventLevel level,
            LoggerOptions options)
        {
            var consoleOptions = options.Console ?? new ConsoleOptions();
            var fileOptions = options.File ?? new FileOptions();
            var seqOptions = options.Seq ?? new SeqOptions();
            if (consoleOptions.Enabled) loggerConfiguration.WriteTo.Console();

            if (fileOptions.Enabled)
            {
                var path = string.IsNullOrWhiteSpace(fileOptions.Path) ? "logs/logs.txt" : fileOptions.Path;
                if (!Enum.TryParse<RollingInterval>(fileOptions.Interval, true, out var interval))
                    interval = RollingInterval.Day;

                loggerConfiguration.WriteTo.File(path, rollingInterval: interval);
            }

            if (seqOptions.Enabled) loggerConfiguration.WriteTo.Seq(seqOptions.Url, apiKey: seqOptions.ApiKey);
        }
    }
}