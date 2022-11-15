using LightWServer.Core.Exceptions;
using LightWServer.Core.HttpContext;
using System.Globalization;

namespace LightWServer.Core.Utils
{
    internal static class RequestParser
    {
        internal static async Task<Request> ReadAsync(Stream networkStream)
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
            var headersCollection = new HeaderCollection();

            foreach (var header in headers)
            {
                if (!string.IsNullOrWhiteSpace(header))
                {
                    var (key, value) = ParseHeader(header);
                    headersCollection.Add(key, value);
                }
            }

            return headersCollection;
        }

        private static (string key, string value) ParseHeader(string header)
        {
            const string HeaderSeparator = ": ";

            var index = header.IndexOf(HeaderSeparator);

            if (index == -1)
                throw new InvalidRequestException("Invalid header format");

            var key = header.Substring(0, index).Trim();
            var value = header.Substring(index + HeaderSeparator.Length).Trim();

            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                throw new InvalidRequestException("Invalid header key or value format");

            return (key, value);
        }

        private static (HttpMethod httpMethod, string path, string httpVersion) ParseStartLine(string? startLine)
        {
            if (string.IsNullOrWhiteSpace(startLine))
                throw new InvalidRequestException("Start line is empty");

            var split = startLine.Split(' ');

            if (split.Length < 2 || split.Length > 3)
                throw new InvalidRequestException("Invalid start line");

            var path = split[1].StartsWith('/') ? split[1].Substring(1) : split[1];
            var method = ParseMethod(split[0]);
            var httpVersion = split.Length == 3 ? ValidateHttpVersion(split[2]) : "0.9";

            return (method, path, httpVersion);
        }

        private static HttpMethod ParseMethod(string httpMethod) => httpMethod switch
        {
            "GET" => HttpMethod.Get,
            _ => throw new MethodNotAllowedException($"Not allowed '{httpMethod}' HTTP method")
        };

        private static string ValidateHttpVersion(string httpVersion)
        {
            const string httpStart = "HTTP/";

            if (string.IsNullOrWhiteSpace(httpVersion) || !httpVersion.StartsWith(httpStart))
                throw new InvalidRequestException("Invalid HTTP version");

            var version = httpVersion.Substring(httpStart.Length);

            if (!double.TryParse(version, NumberStyles.Any, CultureInfo.InvariantCulture, out var _))
                throw new InvalidRequestException("Invalid HTTP version value");

            return version;
        }
    }
}
