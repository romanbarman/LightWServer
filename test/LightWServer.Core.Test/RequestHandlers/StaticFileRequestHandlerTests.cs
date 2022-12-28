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
        [InlineData(".pdf", "application/pdf")]
        [InlineData(".jpg", "image/jpg")]
        public void Handle_If_File_Exist_Should_Return_FileResponse(string extension, string expectedContentType)
        {
            const string RootPath = "Files";

            var fileName = $"Test{extension}";
            var content = new Fixture().Create<string>();

            using var tempFileCreator = new TempFileCreator(RootPath, fileName, content);

            var fileInfo = new FileInfo(tempFileCreator.FullPath);
            var expectedHeaders = CreateHeaderCollection(fileInfo.Length, expectedContentType);

            var underTest = new StaticFileRequestHandler(RootPath);
            var response = underTest.Handle(new Request("1.1", fileName, HttpMethod.Get, HeaderCollection.CreateForRequest())) as FileResponse;

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Path.Combine(RootPath, fileName), response.FilePath);
            Assert.Equal(expectedHeaders.GetHeadersNames(), response.Headers.GetHeadersNames());
            Assert.Equal(expectedHeaders, response.Headers);
        }

        [Fact]
        public void Handle_If_File_Not_Exist_Should_Return_Response_With_Not_Found()
        {
            const string RootPath = "www";

            var fileName = "Test.txt";

            var underTest = new StaticFileRequestHandler(RootPath);
            var response = underTest.Handle(new Request("1.1", fileName, HttpMethod.Get, HeaderCollection.CreateForRequest()));

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Single(response.Headers.GetHeadersNames());
            Assert.Equal("Server", response.Headers.Single().Name);
        }

        private static HeaderCollection CreateHeaderCollection(long contentLength, string contentType)
        {
            var headerCollection = HeaderCollection.CreateForResponse();
            headerCollection.Add("Accept-Ranges", "bytes");
            headerCollection.Add("Content-Length", contentLength.ToString());
            headerCollection.Add("Content-Type", contentType);

            return headerCollection;
        }
    }
}
