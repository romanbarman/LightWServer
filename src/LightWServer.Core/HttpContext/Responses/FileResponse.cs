using System.Net;
using LightWServer.Core.HttpContext.Headers;

namespace LightWServer.Core.HttpContext.Responses
{
    internal sealed class FileResponse : Response
    {
        internal string FilePath { get; }

        internal FileResponse(HttpStatusCode statusCode, IHeaderCollection headerCollection, string filePath)
            : base(statusCode, headerCollection)
        {
            FilePath = filePath;
        }
    }
}
