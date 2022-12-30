using LightWServer.Core.HttpContext;
using LightWServer.Core.HttpContext.Responses;
using LightWServer.Core.RequestHandlers;
using LightWServer.Core.Services.FileOperation;
using Moq;
using System.Net;
using Xunit;

namespace LightWServer.Core.Test.RequestHandlers
{
    public class StaticFileRequestHandlerTests
    {
        private Mock<IFileOperationService> fileOperationService;

        public StaticFileRequestHandlerTests()
        {
            fileOperationService = new Mock<IFileOperationService>();
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
            var fullPath = Path.Combine(RootPath, fileName);
            var fileInfo = new FileInformation(1, extension);
            var expectedHeaders = CreateHeaderCollection(fileInfo.Length, expectedContentType);

            fileOperationService.Setup(s => s.Exists(fullPath)).Returns(true);
            fileOperationService.Setup(s => s.GetFileInfo(fullPath)).Returns(fileInfo);

            var underTest = new StaticFileRequestHandler(RootPath, fileOperationService.Object);
            var response = underTest.Handle(new Request("1.1", fileName, HttpMethod.Get, HeaderCollection.CreateForRequest())) as FileResponse;

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(fullPath, response.FilePath);
            Assert.Equal(expectedHeaders.GetHeadersNames(), response.Headers.GetHeadersNames());
            Assert.Equal(expectedHeaders, response.Headers);

            fileOperationService.Verify();
        }

        [Fact]
        public void Handle_If_File_Not_Exist_Should_Return_Response_With_Not_Found()
        {
            const string RootPath = "www";

            var fileName = "Test.txt";

            fileOperationService.Setup(s => s.Exists(fileName)).Returns(false);

            var underTest = new StaticFileRequestHandler(RootPath, fileOperationService.Object);
            var response = underTest.Handle(new Request("1.1", fileName, HttpMethod.Get, HeaderCollection.CreateForRequest()));

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Single(response.Headers.GetHeadersNames());
            Assert.Equal("Server", response.Headers.Single().Name);

            fileOperationService.Verify();
        }

        private static HeaderCollection CreateHeaderCollection(long contentLength, string contentType)
        {
            var headerCollection = HeaderCollection.CreateForResponse();
            headerCollection.Add(new Header("Accept-Ranges", "bytes"));
            headerCollection.Add(new Header("Content-Length", contentLength.ToString()));
            headerCollection.Add(new Header("Content-Type", contentType));

            return headerCollection;
        }
    }
}
