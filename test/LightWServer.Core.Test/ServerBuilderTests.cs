using Xunit;

namespace LightWServer.Core.Test
{
    public class ServerBuilderTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-2)]
        public void SetPort_Invalid_Port_Should_Throw_Exception(int port)
        {
            var underTest = new ServerBuilder();

            Assert.Throws<ArgumentException>(() => underTest.SetPort(port));
        }

        [Fact]
        public void SetStaticFileRequestHandler_Path_Is_Null_Should_Throw_ArgumentNullException()
        {
            var underTest = new ServerBuilder();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentNullException>(() => underTest.SetStaticFileRequestHandler(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        [Fact]
        public void SetStaticFileRequestHandler_Path_Is_Empty_Should_Throw_ArgumentNullException()
        {
            var underTest = new ServerBuilder();

            Assert.Throws<ArgumentException>(() => underTest.SetStaticFileRequestHandler(""));
        }

        [Fact]
        public void SetLogger_Logger_Is_Null_Should_Throw_ArgumentNullException()
        {
            var underTest = new ServerBuilder();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.Throws<ArgumentNullException>(() => underTest.SetLogger(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
