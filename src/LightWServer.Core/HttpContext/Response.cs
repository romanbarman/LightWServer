using System.Net;
using LightWServer.Core.HttpContext.Headers;

namespace LightWServer.Core.HttpContext
{
    internal class Response
    {
        internal HttpStatusCode StatusCode { get; }
        internal IHeaderCollection Headers { get; }

        internal Response(HttpStatusCode statusCode, IHeaderCollection headerCollection)
        {
            StatusCode = statusCode;
            Headers = headerCollection;
        }
    }
}
