using System.Net;

namespace LightWServer.Core.HttpContext.Responses
{
    internal sealed class FileResponse : Response
    {
        internal string FilePath { get; }

        internal FileResponse(HttpStatusCode statusCode, IHeaderCollection headerCollection, string filePath)
            : base(statusCode, headerCollection)
        {
            if (filePath.Trim().Equals(string.Empty))
                throw new ArgumentException("File path is empty", nameof(filePath));

            FilePath = filePath;
        }
    }
}
