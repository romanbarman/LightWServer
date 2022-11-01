using AutoFixture;
using LightWServer.Core.HttpContext;
using LightWServer.Core.HttpContext.Responses;
using LightWServer.Core.RequestHandlers;
using System.Net;
using Xunit;

namespace LightWServer.Core.Test.RequestHandlers
{
    public class StaticFileRequestHandlerTests
    {
        private const string ServerHeaderName = "Server";
        private const string ServerHeaderValue = "LightWServer/0.0.01";

        [Fact]
        public void Constructor_If_Invalid_Path_Then_Throw_Exception()
        {
            Assert.Throws<ArgumentException>(() => new StaticFileRequestHandler(""));
        }

        [Theory]
        [InlineData(".txt", "text/html")]
        [InlineData(".gif", "image/gif")]
        [InlineData(".jpeg", "image/jpeg")]
        [InlineData(".png", "image/png")]
        [InlineData(".svg", "image/svg+xml")]
        [InlineData(".css", "text/css")]
        [InlineData(".csv", "text/csv")]
        [InlineData(".html", "text/html")]
        [InlineData(".js", "text/javascript")]
        public void Handle_If_File_Exist_Should_Return_FileResponse(string extension, string expectedContentType)
        {
            const string RootPath = "Files";

            var fileName = $"Test{extension}";
            var content = new Fixture().Create<string>();

            using var tempFileCreator = new TempFileCreator(RootPath, fileName, content);

            var fileInfo = new FileInfo(tempFileCreator.FullPath);
            var expectedHeaders = CreateHeaderCollection(fileInfo.Length, expectedContentType);

            var underTest = new StaticFileRequestHandler(RootPath);
            var response = underTest.Handle(new Request("1.1", fileName, HttpMethod.Get, new HeaderCollection())) as FileResponse;

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Path.Combine(RootPath, fileName), response.FilePath);

            Assert.Equal(expectedHeaders.GetKeys(), response.Headers.GetKeys());

            foreach (var header in expectedHeaders.GetKeys())
                Assert.Equal(expectedHeaders.GetValue(header), response.Headers.GetValue(header));
        }

        [Fact]
        public void Handle_If_File_Not_Exist_Should_Return_Response_With_Not_Found()
        {
            const string RootPath = "www";

            var fileName = "Test.txt";

            var underTest = new StaticFileRequestHandler(RootPath);
            var response = underTest.Handle(new Request("1.1", fileName, HttpMethod.Get, new HeaderCollection()));

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Single(response.Headers.GetKeys());
            Assert.Equal(ServerHeaderValue, response.Headers.GetValue(ServerHeaderName));
        }

        private static HeaderCollection CreateHeaderCollection(long contentLength, string contentType)
        {
            var headerCollection = new HeaderCollection();
            headerCollection.Add(ServerHeaderName, ServerHeaderValue);
            headerCollection.Add("Accept-Ranges", "bytes");
            headerCollection.Add("Content-Length", contentLength.ToString());
            headerCollection.Add("Content-Type", contentType);

            return headerCollection;
        }
    }
}
