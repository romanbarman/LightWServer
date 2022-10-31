using System.Net;

namespace LightWServer.Core.HttpContext
{
    internal class Response
    {
        internal HttpStatusCode StatusCode { get; }
        internal IHeaderCollection Headers { get; }

        public Response(HttpStatusCode statusCode, IHeaderCollection headerCollection)
        {
            StatusCode = statusCode;
            Headers = headerCollection ?? throw new ArgumentNullException(nameof(headerCollection));
        }
    }
}
