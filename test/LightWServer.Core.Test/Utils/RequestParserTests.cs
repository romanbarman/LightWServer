using LightWServer.Core.Exceptions;
using LightWServer.Core.HttpContext;
using LightWServer.Core.Utils;
using System.Text;
using Xunit;

namespace LightWServer.Core.Test.Utils
{
    public class RequestParserTests
    {
        [Fact]
        public async Task ReadAsync_If_Start_Line_Is_Null_Should_Thrown_InvalidRequestException()
        {
            using var memoryStream = new MemoryStream();

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => RequestParser.ReadAsync(memoryStream));

            Assert.Equal("Start line is empty", exception.Message);
        }

        [Fact]
        public async Task ReadAsync_If_Start_Line_Is_Empty_Should_Thrown_InvalidRequestException()
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(" "));

            var exception = await Assert.ThrowsAsync<InvalidRequestException>(() => RequestParser.ReadAsync(memoryStream));

            Assert.Equal("Start line is empty", exception.Message);
        }

        [Theory]
        [MemberData(nameof(InvalidData))]
        public async Task ReadAsync_If_Invalid_Request_Should_Thrown_InvalidRequestException(string request,
            Exception expectedException)
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(request));

            var exception = await Assert.ThrowsAsync(expectedException.GetType(), () => RequestParser.ReadAsync(memoryStream));

            Assert.Equal(expectedException.Message, exception.Message);
        }

        [Theory]
        [MemberData(nameof(ValidData))]
        public async Task ReadAsync_If_Valid_Request_Should_Return_Result(string request, object expectedResult)
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(request));

            var result = await RequestParser.ReadAsync(memoryStream);

            var expectedRequest = (Request)expectedResult;

            Assert.Equal(expectedRequest.Path, result.Path);
            Assert.Equal(expectedRequest.HttpVersion, result.HttpVersion);
            Assert.Equal(expectedRequest.HttpMethod, result.HttpMethod);
            Assert.Equal(expectedRequest.Headers, result.Headers);
        }

        public static IEnumerable<object[]> InvalidData =>
        new List<object[]>
        {
            new object[] { "GET", new InvalidRequestException("Invalid start line") },
            new object[] { "GET /Index HTTP/1.1 Test", new InvalidRequestException("Invalid start line") },
            new object[] { "POST /Index HTTP/1.1", new MethodNotAllowedException("Not allowed 'POST' HTTP method") },
            new object[] { "GET /Index HTTPS/1.1", new InvalidRequestException("Invalid HTTP version") },
            new object[] { "GET /Index HTTP/", new InvalidRequestException("Invalid HTTP version value") },
            new object[] { "GET /Index HTTP/A", new InvalidRequestException("Invalid HTTP version value") },
            new object[] { $"GET /Index HTTP/1.1{Environment.NewLine}Service", new InvalidRequestException("Invalid header format") },
            new object[] { $"GET /Index HTTP/1.1{Environment.NewLine}Service; Test", new InvalidRequestException("Invalid header format") },
            new object[] { $"GET /Index HTTP/1.1{Environment.NewLine}Accept-Ranges: bytes{Environment.NewLine}Service; LightWServer/0.0.01", new InvalidRequestException("Invalid header format") },
            new object[] { $"GET /Index HTTP/1.1{Environment.NewLine} : Test", new InvalidRequestException("Invalid header key or value format") },
            new object[] { $"GET /Index HTTP/1.1{Environment.NewLine}Service : ", new InvalidRequestException("Invalid header key or value format") }
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
