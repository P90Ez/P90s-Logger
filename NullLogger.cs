using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P90Ez.Logger
{
    /// <summary>
    /// Used to void log statements, or as a substitute if no Logger is provided. Use <seealso cref="Instance"/>.
    /// </summary>
    public class NullLogger : ILogger
    {
        public static readonly NullLogger Instance = new NullLogger();

        private NullLogger() { }
        public void Log(string Message, ILogger.Severity Severity)
        {
        }
    }
}
