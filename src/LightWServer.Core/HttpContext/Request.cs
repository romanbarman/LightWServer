namespace LightWServer.Core.HttpContext
{
    internal sealed class Request
    {
        internal string HttpVersion { get; }
        internal string Path { get; }
        internal HttpMethod HttpMethod { get; }
        internal IHeaderCollection HeaderCollection { get; }

        internal Request(string httpVersion, string path, HttpMethod httpMethod, IHeaderCollection headerCollection)
        {
            if (string.IsNullOrEmpty(httpVersion))
                throw new ArgumentNullException(nameof(httpVersion));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            HttpVersion = httpVersion;
            Path = path;
            HttpMethod = httpMethod ?? throw new ArgumentNullException(nameof(httpMethod));
            HeaderCollection = headerCollection ?? throw new ArgumentNullException(nameof(headerCollection));
        }
    }
}
