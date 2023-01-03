using LightWServer.Core.Exceptions;
using LightWServer.Core.HttpContext;
using LightWServer.Core.HttpContext.Headers;
using System.Globalization;

namespace LightWServer.Core.Services
{
    internal class RequestReader : IRequestReader
    {
        public async Task<Request> ReadAsync(Stream networkStream)
        {
            using (var reader = new StreamReader(networkStream, leaveOpen: true))
            {
                var startLine = await reader.ReadLineAsync();

                var headers = new List<string>();

                for (var line = await reader.ReadLineAsync(); !string.IsNullOrWhiteSpace(line); line = await reader.ReadLineAsync())
                    headers.Add(line);

                var (httpMethod, path, httpVersion) = ParseStartLine(startLine);
                var headersCollection = ParseHeaders(headers);

                return new Request(httpVersion, path, httpMethod, headersCollection);
            }
        }

        private static IHeaderCollection ParseHeaders(IList<string> headers)
        {
            var headersCollection = HeaderCollection.CreateForRequest();

            foreach (var header in headers)
            {
                if (!string.IsNullOrWhiteSpace(header))
                {
                    headersCollection.Add(Header.Parse(header));
                }
            }

            return headersCollection;
        }

        private static (HttpMethod httpMethod, string path, string httpVersion) ParseStartLine(string? startLine)
        {
            if (string.IsNullOrWhiteSpace(startLine))
                throw new EmptyRequestException();

            var split = startLine.Split(' ');

            if (split.Length < 2 || split.Length > 3)
                throw new InvalidRequestException("Invalid start line.");

            var path = split[1].StartsWith('/') ? split[1].Substring(1) : split[1];
            var method = ParseMethod(split[0]);
            var httpVersion = split.Length == 3 ? ValidateHttpVersion(split[2]) : "0.9";

            return (method, path, httpVersion);
        }

        private static HttpMethod ParseMethod(string httpMethod) => httpMethod switch
        {
            "GET" => HttpMethod.Get,
            _ => throw new MethodNotAllowedException(httpMethod)
        };

        private static string ValidateHttpVersion(string httpVersion)
        {
            const string httpStart = "HTTP/";

            if (string.IsNullOrWhiteSpace(httpVersion) || !httpVersion.StartsWith(httpStart))
                throw new InvalidHttpVersionFormatException(httpVersion);

            var version = httpVersion.Substring(httpStart.Length);

            if (!double.TryParse(version, NumberStyles.Any, CultureInfo.InvariantCulture, out var _))
                throw new InvalidHttpVersionFormatException(httpVersion);

            return version;
        }
    }
}
