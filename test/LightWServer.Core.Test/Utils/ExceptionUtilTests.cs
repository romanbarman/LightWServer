using LightWServer.Core.Exceptions;
using LightWServer.Core.Utils;
using System.Net;
using Xunit;

namespace LightWServer.Core.Test.Utils
{
    public class ExceptionUtilTests
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void ExceptionToResponse_Should_Return_Response(Exception exception, HttpStatusCode expectedStatusCode)
        {
            var response = ExceptionUtil.ExceptionToResponse(exception);

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
