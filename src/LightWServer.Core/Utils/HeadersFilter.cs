using LightWServer.Core.HttpContext;

namespace LightWServer.Core.Utils
{
    internal static class HeadersFilter
    {
        private static readonly string[] HeadersForFilter = new[]
        {
            "Accept", "Accept-Charset", "Accept-Encoding", "Accept-Language", "Cache-Control", "Host", "Referer", "User-Agent"
        };

        internal static IEnumerable<Header> Filter(Request request) => request.Headers
            .Where(h => HeadersForFilter.Contains(h.Name, StringComparer.OrdinalIgnoreCase));
    }
}
