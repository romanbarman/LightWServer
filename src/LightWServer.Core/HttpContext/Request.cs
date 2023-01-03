using LightWServer.Core.HttpContext.Headers;

namespace LightWServer.Core.HttpContext
{
    internal sealed class Request
    {
        internal string HttpVersion { get; }
        internal string Path { get; }
        internal HttpMethod HttpMethod { get; }
        internal IHeaderCollection Headers { get; }

        internal Request(string httpVersion, string path, HttpMethod httpMethod, IHeaderCollection headers)
        {
            HttpVersion = httpVersion;
            Path = path;
            HttpMethod = httpMethod;
            Headers = headers;
        }
    }
}
