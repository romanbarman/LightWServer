using LightWServer.Core.HttpContext;

namespace LightWServer.Core.Services
{
    internal interface IResponseWriter
    {
        Task WriteAsync(Response response, Stream networkStream);
    }
}
