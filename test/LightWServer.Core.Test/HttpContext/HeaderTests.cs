using AutoFixture.Xunit2;
using LightWServer.Core.Exceptions;
using LightWServer.Core.HttpContext.Headers;
using Xunit;

namespace LightWServer.Core.Test.HttpContext
{
    public class HeaderTests
    {
        [Theory]
        [InlineData("Service")]
        [InlineData("Service; Test")]
        [InlineData(" : Test")]
        [InlineData("Service : ")]
        public void Parse_If_Invalid_Header_Should_Thrown_InvalidHeaderFormatException(string header)
        {
            Assert.Throws<InvalidHeaderFormatException>(() => Header.Parse(header));
        }

        [Theory, AutoData]
        public void Parse_If_Valid_Header_Should_Return_Result(string name, string value)
        {
            var header = Header.Parse($"{name}: {value}");

            Assert.Equal(name, header.Name);
            Assert.Equal(value, header.Value);
        }
    }
}
