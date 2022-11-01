using System.Net;

namespace LightWServer.Core.HttpContext
{
    public class Response
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
