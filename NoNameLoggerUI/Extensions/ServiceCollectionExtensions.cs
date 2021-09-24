using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerUI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNoNameLoggerUi(this IServiceCollection services, Action<NoNameLoggerUiOptionsBuilder> optionsBuilder)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            var builder = new NoNameLoggerUiOptionsBuilder(services);
            optionsBuilder.Invoke(builder);

            return services;
        }
    }
}
