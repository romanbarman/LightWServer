using LightWServer.Core.HttpContext;
using LightWServer.Core.Utils;
using Xunit;

namespace LightWServer.Core.Test.Utils
{
    public class HeadersFilterTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Filter_Should_Return_Headers(Request request, HeaderCollection expectedHeaders)
        {
            var headers = HeadersFilter.Filter(request);

            Assert.NotNull(headers);
            Assert.Equal(expectedHeaders, headers);
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new Request("1.0", "test", HttpMethod.Get, new HeaderCollection()), new HeaderCollection() },
            new object[] { new Request("1.0", "test", HttpMethod.Get, new HeaderCollection
            {
                { "Accept", "text/plain" },
                { "Accept-Charset", "utf-8" },
                { "Accept-Encoding", "deflate" },
                { "Accept-Language", "en" },
                { "Cache-Control", "no-cache" },
                { "Pragma", "no-cache" },
                { "Host", "test.com" },
                { "Referer", "test.com" },
                { "Date", "Tue, 15 Nov 1994 08:12:31 GMT" },
                { "User-Agent", "Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20100101 Firefox/4.0.1" },
            }),
            new HeaderCollection
            {
                { "Accept", "text/plain" },
                { "Accept-Charset", "utf-8" },
                { "Accept-Encoding", "deflate" },
                { "Accept-Language", "en" },
                { "Cache-Control", "no-cache" },
                { "Host", "test.com" },
                { "Referer", "test.com" },
                { "User-Agent", "Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20100101 Firefox/4.0.1" },
            } },
            new object[] { new Request("1.0", "test", HttpMethod.Get, new HeaderCollection
            {
                { "Accept", "text/plain" },
                { "Accept-Language", "en" },
                { "Host", "test.com" },
                { "Referer", "test.com" },
                { "User-Agent", "Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20100101 Firefox/4.0.1" },
            }),
            new HeaderCollection
            {
                { "Accept", "text/plain" },
                { "Accept-Language", "en" },
                { "Host", "test.com" },
                { "Referer", "test.com" },
                { "User-Agent", "Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20100101 Firefox/4.0.1" },
            } }
        };
    }
}
