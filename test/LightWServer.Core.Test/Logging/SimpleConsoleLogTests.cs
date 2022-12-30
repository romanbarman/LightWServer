using AutoFixture;
using LightWServer.Core.Logging;
using System.Globalization;
using Xunit;

namespace LightWServer.Core.Test.Logging
{
    public class SimpleConsoleLogTests
    {
        private Fixture fixture;

        private SimpleConsoleLog underTest;

        public SimpleConsoleLogTests()
        {
            fixture = new Fixture();

            underTest = new SimpleConsoleLog();
        }

        [Theory]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.Warning)]
        [InlineData(LogLevel.Error)]
        public void Log_If_Valid_Message_Then_Write_To_Console(LogLevel logLevel)
        {
            var message = fixture.Create<string>();

            var output = new StringWriter();
            Console.SetOut(output);

            underTest.Log(logLevel, message);

            var log = output.ToString();

            Assert.Contains($"- [{logLevel.ToString().ToUpper()}] {message}", log);

            var date = Convert.ToDateTime(log.Substring(0, log.IndexOf(" - [")), CultureInfo.CurrentCulture);
            Assert.InRange(date, DateTime.Now.AddSeconds(-5), DateTime.Now.AddSeconds(5));
        }

        [Theory]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.Warning)]
        [InlineData(LogLevel.Error)]
        public void Log_With_Exception_If_Valid_Message_Then_Write_To_Console(LogLevel logLevel)
        {
            var message = fixture.Create<string>();
            var exception = fixture.Create<Exception>();

            var output = new StringWriter();
            Console.SetOut(output);

            underTest.Log(logLevel, message, exception);

            var log = output.ToString();

            Assert.Contains($"- [{logLevel.ToString().ToUpper()}] {message}{Environment.NewLine}{exception}", log);

            var date = Convert.ToDateTime(log.Substring(0, log.IndexOf(" - [")), CultureInfo.CurrentCulture);
            Assert.InRange(date, DateTime.Now.AddSeconds(-5), DateTime.Now.AddSeconds(5));
        }
    }
}
