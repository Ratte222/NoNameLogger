using System;
using System.Collections.Generic;
using System.Text;

namespace NoNameLogger.Interfaces
{
    public interface ISinksProvider
    {
        ILog CreateLog();
    }
}
