using System.Net;
using LightWServer.Core.HttpContext;
using LightWServer.Core.HttpContext.Responses;

namespace LightWServer.Core.RequestHandlers
{
    internal sealed class StaticFileRequestHandler : IRequestHandler
    {
        private readonly string path;

        internal StaticFileRequestHandler(string path)
        {
            if (path.Trim().Equals(string.Empty))
                throw new ArgumentException("Path is empty", nameof(path));

            this.path = path;
        }

        public Response Handle(Request request)
        {
            var filePath = Path.Combine(path, request.Path);

            return File.Exists(filePath)
                ? Create(filePath)
                : new Response(HttpStatusCode.NotFound, HeaderCollection.CreateForResponse());
        }

        private static FileResponse Create(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            var headerCollection = HeaderCollection.CreateForResponse();
            headerCollection.Add("Accept-Ranges", "bytes");
            headerCollection.Add("Content-Length", fileInfo.Length.ToString());
            headerCollection.Add("Content-Type", GetContentType(fileInfo.Extension));

            return new FileResponse(HttpStatusCode.OK, headerCollection, filePath);
        }

        private static string GetContentType(string extension) => extension switch
        {
            ".gif" => "image/gif",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".svg" => "image/svg+xml",
            ".css" => "text/css",
            ".csv" => "text/csv",
            ".html" => "text/html",
            ".js" => "text/javascript",
            _ => "text/html"
        };
    }
}
