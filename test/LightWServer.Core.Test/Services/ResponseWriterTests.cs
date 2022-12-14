using LightWServer.Core.HttpContext.Responses;
using LightWServer.Core.HttpContext;
using LightWServer.Core.Services;
using System.Net;
using System.Text;
using Xunit;
using Moq;
using LightWServer.Core.Services.FileOperation;
using AutoFixture;
using LightWServer.Core.HttpContext.Headers;

namespace LightWServer.Core.Test.Services
{
    public class ResponseWriterTests
    {
        private Mock<IFileOperationService> fileOperationService;

        private ResponseWriter underTest;

        public ResponseWriterTests()
        {
            fileOperationService = new Mock<IFileOperationService>();

            underTest = new ResponseWriter(fileOperationService.Object);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task WriteResponse_Should_Write_Response(object response, string expectedResult)
        {
            using var memoryStream = new MemoryStream();

            await underTest.WriteAsync((Response)response, memoryStream);

            var result = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.Equal(expectedResult, result);

            fileOperationService.Verify();
        }

        [Fact]
        public async Task WriteResponse_Should_Write_FileResponse()
        {
            var fullPath = Path.Combine("Files", "Test.txt");
            var content = new Fixture().Create<string>();
            var response = new FileResponse(HttpStatusCode.OK, HeaderCollection.CreateForResponse(), fullPath);
            var serverHeader = new ServerHeader();
            var expectedResult = $"HTTP/1.0 200 OK{Environment.NewLine}" +
                $"{serverHeader}{Environment.NewLine}{Environment.NewLine}{content}";

            fileOperationService.Setup(s => s.OpenRead(fullPath)).Returns(new MemoryStream(Encoding.UTF8.GetBytes(content)));

            using var memoryStream = new MemoryStream();
            await underTest.WriteAsync(response, memoryStream);
            var result = Encoding.UTF8.GetString(memoryStream.ToArray());

            Assert.Equal(expectedResult, result);

            fileOperationService.Verify();
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[]
            {
                new Response(HttpStatusCode.OK, HeaderCollection.CreateForResponse()),
                $"HTTP/1.0 200 OK{Environment.NewLine}{new ServerHeader()}{Environment.NewLine}{Environment.NewLine}"
            },
            new object[]
            {
                new Response(HttpStatusCode.Redirect, HeaderCollectionBuilder.Create(new ServerHeader(), new Header("Accept-Ranges", "bytes"))),
                $"HTTP/1.0 302 Redirect{Environment.NewLine}{new ServerHeader()}{Environment.NewLine}Accept-Ranges: bytes{Environment.NewLine}{Environment.NewLine}"
            }
        };
    }
}
