using LightWServer.Core.Exceptions;
using LightWServer.Core.HttpContext;
using LightWServer.Core.Services;
using System.Text;
using Xunit;

namespace LightWServer.Core.Test.Services
{
    public class RequestReaderTests
    {
        private RequestReader underTest;

        public RequestReaderTests()
        {
            underTest = new RequestReader();
        }

        [Fact]
        public async Task ReadAsync_If_Start_Line_Is_Null_Should_Thrown_EmptyRequestException()
        {
            using var memoryStream = new MemoryStream();

            await Assert.ThrowsAsync<EmptyRequestException>(() => underTest.ReadAsync(memoryStream));
        }

        [Fact]
        public async Task ReadAsync_If_Start_Line_Is_Empty_Should_Thrown_EmptyRequestException()
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(" "));

            await Assert.ThrowsAsync<EmptyRequestException>(() => underTest.ReadAsync(memoryStream));
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task ReadAsync_If_Invalid_Request_Should_Thrown_Exception(string request,
            Exception expectedException)
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(request));

            var exception = await Assert.ThrowsAsync(expectedException.GetType(), () => underTest.ReadAsync(memoryStream));

            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ReadAsync_If_Valid_Request_Should_Return_Result(string request, object expectedResult)
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(request));

            var result = await underTest.ReadAsync(memoryStream);

            var expectedRequest = (Request)expectedResult;

            Assert.Equal(expectedRequest.Path, result.Path);
            Assert.Equal(expectedRequest.HttpVersion, result.HttpVersion);
            Assert.Equal(expectedRequest.HttpMethod, result.HttpMethod);
            Assert.Equal(expectedRequest.Headers, result.Headers);
        }

        public static IEnumerable<object[]> InvalidData =>
        new List<object[]>
        {
            new object[] { "GET", new InvalidRequestException("Invalid start line.") },
            new object[] { "GET /Index HTTP/1.1 Test", new InvalidRequestException("Invalid start line.") },
            new object[] { "POST /Index HTTP/1.1", new MethodNotAllowedException("POST") },
            new object[] { "GET /Index HTTPS/1.1", new InvalidHttpVersionFormatException("HTTPS/1.1") },
            new object[] { "GET /Index HTTP/", new InvalidHttpVersionFormatException("HTTP/") },
            new object[] { "GET /Index HTTP/A", new InvalidHttpVersionFormatException("HTTP/A") },
            new object[] { $"GET /Index HTTP/1.1{Environment.NewLine}Accept-Ranges: bytes{Environment.NewLine}Service; LightWServer/0.0.01", new InvalidHeaderFormatException("Service; LightWServer/0.0.01") },
        };

        public static IEnumerable<object[]> ValidData =>
        new List<object[]>
        {
            new object[]
            {
                $"GET /Index HTTP/1.1{Environment.NewLine}Accept-Ranges: bytes{Environment.NewLine}Service: LightWServer/0.0.01",
                new Request("1.1", "Index", HttpMethod.Get,
                    HeaderCollectionBuilder.Create(new Header("Accept-Ranges", "bytes"), new Header("Service", "LightWServer/0.0.01")))
            },
            new object[]
            {
                $"GET Index HTTP/1.1{Environment.NewLine}Service: LightWServer/0.0.01",
                new Request("1.1", "Index", HttpMethod.Get, HeaderCollectionBuilder.Create(new Header("Service", "LightWServer/0.0.01")))
            },
            new object[]
            {
                $"GET /Index",
                new Request("0.9", "Index", HttpMethod.Get, HeaderCollection.CreateForRequest())
            }
        };
    }
}
