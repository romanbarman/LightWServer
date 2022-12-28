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
            if (httpVersion.Trim().Equals(string.Empty))
                throw new ArgumentException("HTTP version is empty", nameof(httpVersion));
            if (path.Trim().Equals(string.Empty))
                throw new ArgumentException("Path is empty", nameof(path));

            HttpVersion = httpVersion;
            Path = path;
            HttpMethod = httpMethod;
            Headers = headers;
        }
    }
}
