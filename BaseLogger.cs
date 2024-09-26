using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace P90Ez.Logger
{
    /// <summary>
    /// A base class for building your own logger.
    /// </summary>
    abstract public class BaseLogger : ILogger
    {
        public abstract void Log(string Message, ILogger.Severity Severity);

        /// <summary>
        /// Formats the message and severity to a printable string. Override this method to create a custom format.
        /// </summary>
        /// <returns>A formatted string, containing logging information.</returns>
        public virtual string FormatMessage(string Message, ILogger.Severity Severity)
        {
            StackTrace Trace = new StackTrace(2, true);
            string Time = DateTime.Now.ToString("HH:mm:ss");
            string Output = string.Empty;

            switch (Severity)
            {
                case ILogger.Severity.Message:
                case ILogger.Severity.Warning:
                    Output = $"[{Time}][{Severity}] {BuildFunctionInformation(Trace.GetFrame(0))} - {Message}";
                    break;
                case ILogger.Severity.Critical:
                    Output = $"[{Time}][{Severity}] {Message}";
                    foreach (StackFrame Frame in Trace.GetFrames())
                    {
                        Output += $"\n\t{BuildFunctionInformation(Frame)}";
                    }
                    break;
            }

            return Output;
        }

        /// <summary>
        /// Extracts namespace, class name, function name and line number from a stack frame and formats it to a printable string. Override this method to create a custom format.
        /// </summary>
        /// <returns>Formatted string, containing function information.</returns>
        public virtual string BuildFunctionInformation(StackFrame? Frame)
        {
            MethodBase? Function = null;
            if (Frame == null || (Function = Frame.GetMethod()) == null) return "{Unknown Function}";

            string FunctionName = Function.Name;
            string ClassName = Function.ReflectedType?.FullName ?? "{Unknown Class}";
            string LineNumber = Frame.GetFileLineNumber().ToString();

            return $"{ClassName}.{FunctionName}:{LineNumber}";
        }
    }
}
