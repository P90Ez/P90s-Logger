using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P90Ez.Logger
{
    /// <summary>
    /// Prints all messages, the severity and it's origin to a file. Thread safe.
    /// </summary>
    public class FileLogger : BaseLogger
    {
        public string OutputFile { get; }

        public int LogBufferSize { get; }

        private ConcurrentQueue<string> Logs = new ConcurrentQueue<string>();

        private bool WriterRunning = false;

        private object Lock = new object();

        private Task? WriterTask = null;

        /// <summary>
        /// Creates a new FileLogger. The logs are written to the file when the number of logs exceeds the specified amount, or when the program exits.
        /// </summary>
        /// <param name="OutputFile">File name or path to write to. All logs will be appended.</param>
        /// <param name="LogBufferSize">Number of logs until buffer will be emptied.</param>
        public FileLogger(string OutputFile, int LogBufferSize = 100)
        {
            this.OutputFile = OutputFile;
            this.LogBufferSize = LogBufferSize;

            AppDomain.CurrentDomain.ProcessExit += OnExit;
        }

        /// <summary>
        /// Gets called when the program exits. Makes sure the logs are written to the file.
        /// </summary>
        private void OnExit(object? sender, EventArgs e)
        {
            if (WriterRunning && WriterTask != null)
            {
                WriterTask.Wait();
            }
            else
            {
                WriteLogsToFile();
            }
        }

        /// <summary>
        /// Writes logs to the specified file, using a stream. Can only be run once at a time.
        /// </summary>
        private void WriteLogsToFile()
        {
            lock (Lock)
            {
                if (WriterRunning) return;
                WriterRunning = true;
            }

            try
            {
                using (StreamWriter Writer = new StreamWriter(OutputFile, true))
                {
                    string? Line;
                    while (Logs.TryDequeue(out Line) && Line != null)
                    {
                        Writer.WriteLine(Line);
                    }
                }
            }
            catch { }

            lock (Lock)
            {
                WriterRunning = false;
            }
        }

        public sealed override void Log(string Message, ILogger.Severity Severity)
        {
            Logs.Enqueue(FormatMessage(Message, Severity));

            lock (Lock)
            {
                if (Logs.Count >= LogBufferSize && !WriterRunning) WriterTask = Task.Run(() => WriteLogsToFile());
            }
        }
    }

    public class CustomFormatLogger : ConsoleLogger
    {
        public override string FormatMessage(string Message, ILogger.Severity Severity)
        {
            //implement body for custom formatting
        }
    }
}
