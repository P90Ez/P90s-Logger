namespace P90Ez.Logger
{
    public interface ILogger
    {
        public enum Severity
        {
            Message,
            Warning,
            Critical
        }

        /// <summary>
        /// Logs the provided message and severity of the message.
        /// </summary>
        public void Log(string Message, Severity Severity);
    }
}
