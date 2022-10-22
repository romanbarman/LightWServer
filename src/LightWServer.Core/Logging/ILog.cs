namespace LightWServer.Core.Logging
{
    public interface ILog
    {
        void Log(LogLevel logLevel, string message);
        void Log(LogLevel logLevel, string message, Exception exception);
    }
}
