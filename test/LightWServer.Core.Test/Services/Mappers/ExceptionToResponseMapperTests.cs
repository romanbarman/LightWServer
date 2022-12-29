using LightWServer.Core.Exceptions;
using LightWServer.Core.Services.Mappers;
using System.Net;
using Xunit;

namespace LightWServer.Core.Test.Services.Mappers
{
    public class ExceptionToResponseMapperTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void ExceptionToResponse_Should_Return_Response(Exception exception, HttpStatusCode expectedStatusCode)
        {
            var response = new ExceptionToResponseMapper().Map(exception);

            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new MethodNotAllowedException("Test"), HttpStatusCode.MethodNotAllowed },
            new object[] { new InvalidRequestException("Test"), HttpStatusCode.NotImplemented },
            new object[] { new Exception("Test"), HttpStatusCode.InternalServerError }
        };
    }
}
