using AutoFixture.Xunit2;
using LightWServer.Core.Exceptions;
using LightWServer.Core.HttpContext.Headers;
using Xunit;

namespace LightWServer.Core.Test.HttpContext
{
    public class HeaderCollectionTests
    {
        private HeaderCollection underTest;

        public HeaderCollectionTests()
        {
            underTest = HeaderCollection.CreateForRequest();
        }

        [Theory, AutoData]
        public void Add_If_Same_Key_Then_Last_Value(string key, string value1, string value2)
        {
            var header1 = new Header(key, value1);
            var header2 = new Header(key, value2);

            underTest.Add(header1);
            underTest.Add(header2);

            Assert.Equal(header2, underTest.Get(key));
        }

        [Theory, AutoData]
        public void Add_If_Same_Key_With_Different_Register_Then_Last_Value(string key, string value1, string value2)
        {
            var header1 = new Header(key.ToLower(), value1);
            var header2 = new Header(key, value2);

            underTest.Add(header1);
            underTest.Add(header2);

            Assert.Equal(header2, underTest.Get(key.ToUpper()));
        }

        [Theory, AutoData]
        public void Contains_If_Key_Exist_Then_True(KeyValuePair<string, string> header1, KeyValuePair<string, string> header2)
        {
            underTest.Add(new Header(header1.Key, header1.Value));
            underTest.Add(new Header(header2.Key, header2.Value));

            Assert.True(underTest.Contains(header1.Key));
        }

        [Theory, AutoData]
        public void Contains_If_Key_Not_Exist_Then_False(KeyValuePair<string, string> header, string keyForSearch)
        {
            underTest.Add(new Header(header.Key, header.Value));

            Assert.False(underTest.Contains(keyForSearch));
        }

        [Theory, AutoData]
        public void Contains_If_Key_Another_Case_Exist_Then_True(KeyValuePair<string, string> header1, KeyValuePair<string, string> header2)
        {
            underTest.Add(new Header(header1.Key, header1.Value));
            underTest.Add(new Header(header2.Key, header2.Value));

            Assert.True(underTest.Contains(header1.Key.ToLower()));
        }

        [Fact]
        public void GetHeadersNames_If_No_Headers_Then_Empty()
        {
            Assert.False(underTest.GetHeadersNames().Any());
        }

        [Theory, AutoData]
        public void GetHeadersNames_If_Headers_Exists_Then_Return_All_Keys(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
                underTest.Add(new Header(header.Key, header.Value));

            Assert.Equal(headers.Keys, underTest.GetHeadersNames());
        }

        [Theory, AutoData]
        public void Get_If_Header_Not_Exists_Then_Throw_Exception(Dictionary<string, string> headers, string key)
        {
            foreach (var header in headers)
                underTest.Add(new Header(header.Key, header.Value));

            Assert.Throws<HeaderNotExistException>(() => underTest.Get(key));
        }

        [Fact]
        public void CreateForResponse_Should_Return_With_Header()
        {
            var header = new ServerHeader();

            var result = HeaderCollection.CreateForResponse();

            Assert.Single(result.GetHeadersNames());
            Assert.True(result.Contains(header.Name));
            Assert.Equal(header, result.Get(header.Name));
        }

        [Fact]
        public void CreateForRequest_Should_Return_Without_Headers()
        {
            var result = HeaderCollection.CreateForRequest();

            Assert.Empty(result.GetHeadersNames());
        }

        [Theory, AutoData]
        public void GetEnumerator_Should_Return_Values(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
                underTest.Add(new Header(header.Key, header.Value));

            var result = underTest.GetEnumerator();

            while (result.MoveNext())
            {
                var current = result.Current;

                Assert.True(headers.ContainsKey(current.Name));
                Assert.Equal(headers[current.Name], current.Value);
            }
        }
    }
}
