using LightWServer.Core.HttpContext;
using LightWServer.Core.Utils;
using Xunit;

namespace LightWServer.Core.Test.Utils
{
    public class HeadersFilterTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Filter_Should_Return_Headers(Request request, object expectedHeaders)
        {
            var headers = HeadersFilter.Filter(request);

            Assert.NotNull(headers);
            Assert.Equal(expectedHeaders, headers);
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new Request("1.0", "test", HttpMethod.Get, HeaderCollectionBuilder.Create()), HeaderCollectionBuilder.Create() },
            new object[] { new Request("1.0", "test", HttpMethod.Get, HeaderCollectionBuilder.Create(
                new Header("Accept", "text/plain"),
                new Header("Accept-Charset", "utf-8"),
                new Header("Accept-Encoding", "deflate"),
                new Header("Accept-Language", "en"),
                new Header("Cache-Control", "no-cache"),
                new Header("Pragma", "no-cache"),
                new Header("Host", "test.com"),
                new Header("Referer", "test.com"),
                new Header("Date", "Tue, 15 Nov 1994 08:12:31 GMT"),
                new Header("User-Agent", "Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20100101 Firefox/4.0.1")
            )),
            HeaderCollectionBuilder.Create(
                new Header("Accept", "text/plain"),
                new Header("Accept-Charset", "utf-8"),
                new Header("Accept-Encoding", "deflate"),
                new Header("Accept-Language", "en"),
                new Header("Cache-Control", "no-cache"),
                new Header("Host", "test.com"),
                new Header("Referer", "test.com"),
                new Header("User-Agent", "Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20100101 Firefox/4.0.1")
            ) },
            new object[] { new Request("1.0", "test", HttpMethod.Get, HeaderCollectionBuilder.Create(
                new Header("Accept", "text/plain"),
                new Header("Accept-Language", "en"),
                new Header("Host", "test.com"),
                new Header("Referer", "test.com"),
                new Header("User-Agent", "Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20100101 Firefox/4.0.1")
            )),
            HeaderCollectionBuilder.Create(
                new Header("Accept", "text/plain"),
                new Header("Accept-Language", "en"),
                new Header("Host", "test.com"),
                new Header("Referer", "test.com"),
                new Header("User-Agent", "Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20100101 Firefox/4.0.1")
            ) }
        };
    }
}
