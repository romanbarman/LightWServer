using System.Net;
using LightWServer.Core.HttpContext;
using LightWServer.Core.HttpContext.Headers;
using LightWServer.Core.HttpContext.Responses;
using LightWServer.Core.Services.FileOperation;

namespace LightWServer.Core.RequestHandlers
{
    internal sealed class StaticFileRequestHandler : IRequestHandler
    {
        private readonly string path;
        private readonly IFileOperationService fileOperationService;

        internal StaticFileRequestHandler(string path, IFileOperationService fileOperationService)
        {
            this.path = path;
            this.fileOperationService = fileOperationService;
        }

        public Response Handle(Request request)
        {
            var filePath = Path.Combine(path, request.Path);

            return fileOperationService.Exists(filePath)
                ? Create(filePath)
                : new Response(HttpStatusCode.NotFound, HeaderCollection.CreateForResponse());
        }

        private FileResponse Create(string filePath)
        {
            var fileInfo = fileOperationService.GetFileInfo(filePath);

            var headerCollection = HeaderCollection.CreateForResponse();
            headerCollection.Add(new Header("Accept-Ranges", "bytes"));
            headerCollection.Add(new Header("Content-Length", fileInfo.Length.ToString()));
            headerCollection.Add(new Header("Content-Type", GetContentType(fileInfo.Extension)));

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
            ".pdf" => "application/pdf",
            ".jpg" => "image/jpg",
            _ => "text/html"
        };
    }
}
