using System.Globalization;
using System.Net;
using System.Text;

namespace LightWServer.Core.Logging
{
    internal sealed class SimpleConsoleLog : ILog
    {
        public void Log(LogLevel logLevel, string message)
        {
            if (message.Trim().Equals(string.Empty))
                throw new ArgumentException("Message is empty", nameof(message));

            Write(logLevel, Format(logLevel, message));
        }

        public void Log(LogLevel logLevel, string message, Exception exception)
        {
            if (message.Trim().Equals(string.Empty))
                throw new ArgumentException("Message is empty", nameof(message));

            Write(logLevel, Format(logLevel, message, exception));
        }

        private static void Write(LogLevel logLevel, string log)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = GetConsoleColor(logLevel);
            Console.WriteLine(log);
            Console.ForegroundColor = previousColor;
        }

        private static string Format(LogLevel logLevel, string message, Exception? exception = null)
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString(CultureInfo.CurrentCulture));
            sb.Append(" - [");
            sb.Append(logLevel.ToString().ToUpper());
            sb.Append("] ");
            sb.AppendLine(message);

            if (exception != null)
            {
                sb.Append(exception);
            }

            return sb.ToString();
        }

        private static ConsoleColor GetConsoleColor(LogLevel logLevel) => logLevel switch
        {
            LogLevel.Trace => ConsoleColor.Gray,
            LogLevel.Debug => ConsoleColor.Green,
            LogLevel.Information => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.DarkYellow,
            LogLevel.Error => ConsoleColor.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
    }
}
