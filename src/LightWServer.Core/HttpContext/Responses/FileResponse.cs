using System.Net;

namespace LightWServer.Core.HttpContext.Responses
{
    internal class FileResponse : Response
    {
        public string FilePath { get; }

        public FileResponse(HttpStatusCode statusCode, IHeaderCollection headerCollection, string filePath)
            : base(statusCode, headerCollection)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            FilePath = filePath;
        }
    }
}
