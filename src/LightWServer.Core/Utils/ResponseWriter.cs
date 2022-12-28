using LightWServer.Core.HttpContext;
using LightWServer.Core.HttpContext.Responses;

namespace LightWServer.Core.Utils
{
    internal static class ResponseWriter
    {
        internal static async Task WriteResponse(Response response, Stream networkStream)
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
                using (var fileStream = File.OpenRead(fileResponse.FilePath))
                {
                    await fileStream.CopyToAsync(networkStream);
                }
            }
        }
    }
}
