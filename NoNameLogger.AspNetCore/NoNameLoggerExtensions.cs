using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using NoNameLogger.AspNetCore.Config;
using NoNameLogger.AspNetCore.Providers;

namespace NoNameLogger.AspNetCore
{
    public static class NoNameLoggerExtensions
    {
        public static ILoggingBuilder AddColorConsoleLogger(
        this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, ColorConsoleLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <ColorConsoleLoggerConfiguration, ColorConsoleLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddColorConsoleLogger(
            this ILoggingBuilder builder,
            Action<ColorConsoleLoggerConfiguration> configure)
        {
            builder.AddColorConsoleLogger();
            builder.Services.Configure(configure);

            return builder;
        }

        public static ILoggingBuilder AddNoNameFileLogger(
        this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, NoNameLoggerFileProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <FileConfiguration, NoNameLoggerFileProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddNoNameFileLogger(
            this ILoggingBuilder builder,
            Action<FileConfiguration> configure)
        {
            builder.AddNoNameFileLogger();
            builder.Services.Configure(configure);

            return builder;
        }

    }
}
