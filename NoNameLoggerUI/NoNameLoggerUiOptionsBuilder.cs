using Microsoft.Extensions.DependencyInjection;
using NoNameLoggerUI.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerUI
{
    public class NoNameLoggerUiOptionsBuilder: INoNameLoggerUiOptionsBuilder
    {
        private readonly IServiceCollection _services;

        IServiceCollection INoNameLoggerUiOptionsBuilder.Services => _services;
        public NoNameLoggerUiOptionsBuilder(IServiceCollection services)
        {
            _services = services;
        }

    }
}
