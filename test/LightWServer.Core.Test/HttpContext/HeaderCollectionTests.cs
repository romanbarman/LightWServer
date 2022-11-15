using AutoFixture.Xunit2;
using LightWServer.Core.HttpContext;
using Xunit;

namespace LightWServer.Core.Test.HttpContext
{
    public class HeaderCollectionTests
    {
        [Theory, AutoData]
        public void Add_If_Invalid_Key_Then_Throw_Exception(string value)
        {
            var underTest = new HeaderCollection();

            Assert.Throws<ArgumentException>(() => underTest.Add("", value));
        }

        [Theory, AutoData]
        public void Add_If_Invalid_Value_Then_Throw_Exception(string key)
        {
            var underTest = new HeaderCollection();

            Assert.Throws<ArgumentException>(() => underTest.Add(key, ""));
        }

        [Theory, AutoData]
        public void Add_If_Same_Key_Then_Last_Value(string key, string value1, string value2)
        {
            var underTest = new HeaderCollection();

            underTest.Add(key, value1);
            underTest.Add(key, value2);

            Assert.Equal(value2, underTest.GetValue(key));
        }

        [Theory, AutoData]
        public void Add_If_Same_Key_With_Different_Register_Then_Last_Value(string key, string value1, string value2)
        {
            var underTest = new HeaderCollection();

            underTest.Add(key.ToLower(), value1);
            underTest.Add(key, value2);

            Assert.Equal(value2, underTest.GetValue(key.ToUpper()));
        }

        [Fact]
        public void ContainsKey_If_Invalid_Key_Then_Throw_Exception()
        {
            var underTest = new HeaderCollection();

            Assert.Throws<ArgumentException>(() => underTest.ContainsKey(""));
        }

        [Theory, AutoData]
        public void ContainsKey_If_Key_Exist_Then_True(KeyValuePair<string, string> header1, KeyValuePair<string, string> header2)
        {
            var underTest = new HeaderCollection();
            underTest.Add(header1.Key, header1.Value);
            underTest.Add(header2.Key, header2.Value);

            Assert.True(underTest.ContainsKey(header1.Key));
        }

        [Theory, AutoData]
        public void ContainsKey_If_Key_Not_Exist_Then_False(KeyValuePair<string, string> header, string keyForSearch)
        {
            var underTest = new HeaderCollection();
            underTest.Add(header.Key, header.Value);

            Assert.False(underTest.ContainsKey(keyForSearch));
        }

        [Theory, AutoData]
        public void ContainsKey_If_Key_Another_Case_Exist_Then_True(KeyValuePair<string, string> header1, KeyValuePair<string, string> header2)
        {
            var underTest = new HeaderCollection();
            underTest.Add(header1.Key, header1.Value);
            underTest.Add(header2.Key, header2.Value);

            Assert.True(underTest.ContainsKey(header1.Key.ToLower()));
        }

        [Fact]
        public void GetKeys_If_No_Headers_Then_Empty()
        {
            var underTest = new HeaderCollection();

            Assert.False(underTest.GetKeys().Any());
        }

        [Theory, AutoData]
        public void GetKeys_If_Headers_Exists_Then_Return_All_Keys(Dictionary<string, string> headers)
        {
            var underTest = new HeaderCollection();

            foreach (var header in headers)
                underTest.Add(header.Key, header.Value);

            Assert.Equal(headers.Keys, underTest.GetKeys());
        }

        [Fact]
        public void GetValue_If_Invalid_Key_Then_Throw_Exception()
        {
            var underTest = new HeaderCollection();

            Assert.Throws<ArgumentException>(() => underTest.GetValue(""));
        }

        [Theory, AutoData]
        public void GetValue_If_Header_Not_Exists_Then_Throw_Exception(Dictionary<string, string> headers, string key)
        {
            var underTest = new HeaderCollection();

            foreach (var header in headers)
                underTest.Add(header.Key, header.Value);

            Assert.Throws<ArgumentOutOfRangeException>(() => underTest.GetValue(key));
        }

        [Fact]
        public void CreateForResponse_Should_Return_With_Header()
        {
            const string Header = "Server";

            var result = HeaderCollection.CreateForResponse();

            Assert.Single(result.GetKeys());
            Assert.True(result.ContainsKey(Header));
            Assert.Equal("LightWServer/0.0.01", result.GetValue(Header));
        }

        [Theory, AutoData]
        public void GetEnumerator_Should_Return_Values(Dictionary<string, string> headers)
        {
            var underTest = new HeaderCollection();

            foreach (var header in headers)
                underTest.Add(header.Key, header.Value);

            var result = underTest.GetEnumerator();

            while (result.MoveNext())
            {
                var current = result.Current;

                Assert.True(headers.ContainsKey(current.Key));
                Assert.Equal(headers[current.Key], current.Value);
            }
        }
    }
}
