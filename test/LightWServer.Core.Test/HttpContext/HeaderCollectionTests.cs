using AutoFixture.Xunit2;
using LightWServer.Core.Exceptions;
using LightWServer.Core.HttpContext;
using Xunit;

namespace LightWServer.Core.Test.HttpContext
{
    public class HeaderCollectionTests
    {
        [Theory, AutoData]
        public void Add_If_Same_Key_Then_Last_Value(string key, string value1, string value2)
        {
            var underTest = HeaderCollection.CreateForRequest();

            underTest.Add(new Header(key, value1));
            underTest.Add(new Header(key, value2));

            Assert.Equal(value2, underTest.Get(key).Value);
        }

        [Theory, AutoData]
        public void Add_If_Same_Key_With_Different_Register_Then_Last_Value(string key, string value1, string value2)
        {
            var underTest = HeaderCollection.CreateForRequest();

            underTest.Add(new Header(key.ToLower(), value1));
            underTest.Add(new Header(key, value2));

            Assert.Equal(value2, underTest.Get(key.ToUpper()).Value);
        }

        [Theory, AutoData]
        public void Contains_If_Key_Exist_Then_True(KeyValuePair<string, string> header1, KeyValuePair<string, string> header2)
        {
            var underTest = HeaderCollection.CreateForRequest();
            underTest.Add(new Header(header1.Key, header1.Value));
            underTest.Add(new Header(header2.Key, header2.Value));

            Assert.True(underTest.Contains(header1.Key));
        }

        [Theory, AutoData]
        public void Contains_If_Key_Not_Exist_Then_False(KeyValuePair<string, string> header, string keyForSearch)
        {
            var underTest = HeaderCollection.CreateForRequest();
            underTest.Add(new Header(header.Key, header.Value));

            Assert.False(underTest.Contains(keyForSearch));
        }

        [Theory, AutoData]
        public void Contains_If_Key_Another_Case_Exist_Then_True(KeyValuePair<string, string> header1, KeyValuePair<string, string> header2)
        {
            var underTest = HeaderCollection.CreateForRequest();
            underTest.Add(new Header(header1.Key, header1.Value));
            underTest.Add(new Header(header2.Key, header2.Value));

            Assert.True(underTest.Contains(header1.Key.ToLower()));
        }

        [Fact]
        public void GetHeadersNames_If_No_Headers_Then_Empty()
        {
            var underTest = HeaderCollection.CreateForRequest();

            Assert.False(underTest.GetHeadersNames().Any());
        }

        [Theory, AutoData]
        public void GetHeadersNames_If_Headers_Exists_Then_Return_All_Keys(Dictionary<string, string> headers)
        {
            var underTest = HeaderCollection.CreateForRequest();

            foreach (var header in headers)
                underTest.Add(new Header(header.Key, header.Value));

            Assert.Equal(headers.Keys, underTest.GetHeadersNames());
        }

        [Theory, AutoData]
        public void GetValue_If_Header_Not_Exists_Then_Throw_Exception(Dictionary<string, string> headers, string key)
        {
            var underTest = HeaderCollection.CreateForRequest();

            foreach (var header in headers)
                underTest.Add(new Header(header.Key, header.Value));

            Assert.Throws<HeaderNotExistException>(() => underTest.Get(key));
        }

        [Fact]
        public void CreateForResponse_Should_Return_With_Header()
        {
            const string Header = "Server";

            var result = HeaderCollection.CreateForResponse();

            Assert.Single(result.GetHeadersNames());
            Assert.True(result.Contains(Header));
            Assert.Equal("LightWServer/0.0.01", result.Get(Header).Value);
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
            var underTest = HeaderCollection.CreateForRequest();

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
