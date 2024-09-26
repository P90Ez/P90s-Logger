using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace P90Ez.Logger
{
    /// <summary>
    /// Prints all messages, the severity and it's origin to the console. Thread safe.
    /// </summary>
    public class ConsoleLogger : BaseLogger
    {
        public sealed override void Log(string Message, ILogger.Severity Severity)
        {
            Console.WriteLine(FormatMessage(Message, Severity));
        }
    }
}
