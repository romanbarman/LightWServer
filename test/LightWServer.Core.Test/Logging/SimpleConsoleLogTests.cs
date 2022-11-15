using AutoFixture;
using AutoFixture.Xunit2;
using LightWServer.Core.Logging;
using System.Globalization;
using Xunit;

namespace LightWServer.Core.Test.Logging
{
    public class SimpleConsoleLogTests
    {
        private Fixture fixture;

        public SimpleConsoleLogTests()
        {
            fixture = new Fixture();
        }

        [Theory, AutoData]
        public void Log_If_Invalid_Message_Then_Throw_Exception(LogLevel logLevel)
        {
            var underTest = new SimpleConsoleLog();

            Assert.Throws<ArgumentException>(() => underTest.Log(logLevel, ""));
        }

        [Theory]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.Warning)]
        [InlineData(LogLevel.Error)]
        public void Log_If_Valid_Message_Then_Write_To_Console(LogLevel logLevel)
        {
            var underTest = new SimpleConsoleLog();
            var message = fixture.Create<string>();

            var output = new StringWriter();
            Console.SetOut(output);

            underTest.Log(logLevel, message);

            var log = output.ToString();

            Assert.Contains($"- [{logLevel.ToString().ToUpper()}] {message}", log);

            var date = Convert.ToDateTime(log.Substring(0, log.IndexOf(" - [")), CultureInfo.CurrentCulture);
            Assert.InRange(date, DateTime.Now.AddSeconds(-5), DateTime.Now.AddSeconds(5));
        }

        [Theory, AutoData]
        public void Log_With_Exception_If_Invalid_Message_Then_Throw_Exception(LogLevel logLevel, Exception exception)
        {
            var underTest = new SimpleConsoleLog();

            Assert.Throws<ArgumentException>(() => underTest.Log(logLevel, "", exception));
        }

        [Theory]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.Warning)]
        [InlineData(LogLevel.Error)]
        public void Log_With_Exception_If_Valid_Message_Then_Write_To_Console(LogLevel logLevel)
        {
            var underTest = new SimpleConsoleLog();
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
