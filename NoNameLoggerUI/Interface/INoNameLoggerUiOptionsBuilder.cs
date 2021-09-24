using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLoggerUI.Interface
{
    public interface INoNameLoggerUiOptionsBuilder
    {
        IServiceCollection Services { get; }
    }
}
