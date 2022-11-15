using AutoFixture;
using LightWServer.Core.HttpContext;
using LightWServer.Core.HttpContext.Responses;
using LightWServer.Core.Utils;
using System.Net;
using System.Text;
using Xunit;

namespace LightWServer.Core.Test.Utils
{
    public class ResponseWriterTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public async Task WriteResponse_Should_Write_Response(Response response, string expectedResult)
        {
            using var memoryStream = new MemoryStream();

            await ResponseWriter.WriteResponse(response, memoryStream);

            var result = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WriteResponse_Should_Write_FileResponse()
        {
            const string RootPath = "Files";

            var fileName = $"Test.txt";
            var content = new Fixture().Create<string>();

            var response = new FileResponse(HttpStatusCode.OK, HeaderCollection.CreateForResponse(), Path.Combine(RootPath, fileName));
            var expectedResult = $"HTTP/1.0 200 OK{Environment.NewLine}" +
                $"Server: LightWServer/0.0.01{Environment.NewLine}{Environment.NewLine}{content}";

            using var tempFileCreator = new TempFileCreator(RootPath, fileName, content);
            using var memoryStream = new MemoryStream();

            await ResponseWriter.WriteResponse(response, memoryStream);

            var result = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[]
            {
                new Response(HttpStatusCode.OK, HeaderCollection.CreateForResponse()),
                $"HTTP/1.0 200 OK{Environment.NewLine}Server: LightWServer/0.0.01{Environment.NewLine}{Environment.NewLine}"
            },
            new object[]
            {
                new Response(HttpStatusCode.Redirect, new HeaderCollection { { "Server", "LightWServer/0.0.01" }, { "Accept-Ranges", "bytes" } }),
                $"HTTP/1.0 302 Redirect{Environment.NewLine}Server: LightWServer/0.0.01{Environment.NewLine}Accept-Ranges: bytes{Environment.NewLine}{Environment.NewLine}"
            },
            new object[]
            {
                new Response(HttpStatusCode.NotFound, new HeaderCollection()),
                $"HTTP/1.0 404 NotFound{Environment.NewLine}{Environment.NewLine}"
            }
        };
    }
}
