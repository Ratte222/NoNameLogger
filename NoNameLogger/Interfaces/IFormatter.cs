using NoNameLogger.Events;
using NoNameLogger.Model;
using System.IO;

namespace NoNameLogger.Interfaces
{
    public interface IFormatter
    {
        void Serialize(StreamWriter streamWriter, Log log);
        void Serialize(TextWriter textWriter, Log log);
    }
}