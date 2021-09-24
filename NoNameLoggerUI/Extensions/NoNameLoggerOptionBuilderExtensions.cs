using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using NoNameLoggerUI.Middleware;

namespace NoNameLoggerUI.Extensions
{
    public static class NoNameLoggerOptionBuilderExtensions
    {
        public static IApplicationBuilder UseNoNameLoggerUI(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));
            return applicationBuilder.UseMiddleware<NoNameLoggerUiMidlleware>();
        }
    }
}
