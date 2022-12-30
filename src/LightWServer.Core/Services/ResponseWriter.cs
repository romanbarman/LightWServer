using LightWServer.Core.HttpContext;
using LightWServer.Core.HttpContext.Responses;
using LightWServer.Core.Services.FileOperation;

namespace LightWServer.Core.Services
{
    internal sealed class ResponseWriter : IResponseWriter
    {
        private readonly IFileOperationService fileOperationService;

        internal ResponseWriter(IFileOperationService fileOperationService)
        {
            this.fileOperationService = fileOperationService;
        }

        public async Task WriteAsync(Response response, Stream networkStream)
        {
            using (var writer = new StreamWriter(networkStream, leaveOpen: true))
            {
                await writer.WriteLineAsync($"HTTP/1.0 {(int)response.StatusCode} {response.StatusCode}");

                foreach (var header in response.Headers)
                {
                    await writer.WriteLineAsync($"{header.Name}: {header.Value}");
                }

                await writer.WriteLineAsync();
            }

            if (response is FileResponse fileResponse)
            {
                using (var fileStream = fileOperationService.OpenRead(fileResponse.FilePath))
                {
                    await fileStream.CopyToAsync(networkStream);
                }
            }
        }
    }
}
