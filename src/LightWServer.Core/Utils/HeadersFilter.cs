using LightWServer.Core.HttpContext;

namespace LightWServer.Core.Utils
{
    internal static class HeadersFilter
    {
        private static readonly string[] HeadersForFilter = new[]
        {
            "Accept", "Accept-Charset", "Accept-Encoding", "Accept-Language", "Cache-Control", "Host", "Referer", "User-Agent"
        };

        internal static IHeaderCollection Filter(Request request)
        {
            var result = new HeaderCollection();

            foreach (var header in request.Headers)
            {
                if (HeadersForFilter.Contains(header.Key, StringComparer.OrdinalIgnoreCase))
                    result.Add(header.Key, header.Value);
            }

            return result;
        }
    }
}
