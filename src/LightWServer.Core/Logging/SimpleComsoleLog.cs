using System.Globalization;
using System.Text;

namespace LightWServer.Core.Logging
{
    internal sealed class SimpleComsoleLog : ILog
    {
        public void Log(LogLevel logLevel, string message)
        {
            Console.ForegroundColor = GetConsoleColor(logLevel);
            Console.WriteLine(Format(logLevel, message));
        }

        public void Log(LogLevel logLevel, string message, Exception exception)
        {
            Console.ForegroundColor = GetConsoleColor(logLevel);
            Console.WriteLine(Format(logLevel, message, exception));
        }

        private static StringBuilder Format(LogLevel logLevel, string message)
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString(CultureInfo.CurrentCulture));
            sb.Append(" - [");
            sb.Append(logLevel.ToString().ToUpper());
            sb.Append("] ");
            sb.Append(message);

            return sb;
        }

        private static StringBuilder Format(LogLevel logLevel, string message, Exception exception)
        {
            var sb = Format(logLevel, message);
            sb.AppendLine("Exception: ");
            sb.Append(exception);

            return sb;
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
