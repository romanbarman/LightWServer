using LightWServer.Core.Logging;
using LightWServer.Core.RequestHandlers;
using LightWServer.Core.Services;
using LightWServer.Core.Services.Mappers;
using Moq;
using Xunit;

namespace LightWServer.Core.Test
{
    public class LightWServerHostTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(-2)]
        public void Constructor_Invalid_Port_Should_Throw_Exception(int port)
        {
            Assert.Throws<ArgumentException>(() => new LightWServerHost( new Mock<IExceptionToResponseMapper>().Object,
                new Mock<IRequestReader>().Object, new Mock<IResponseWriter>().Object, new Mock<IRequestHandler>().Object, new Mock<ILog>().Object, port));
        }
    }
}
