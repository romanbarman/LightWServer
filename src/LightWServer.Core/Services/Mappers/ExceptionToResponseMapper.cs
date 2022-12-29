using LightWServer.Core.Exceptions;
using LightWServer.Core.HttpContext;
using System.Net;

namespace LightWServer.Core.Services.Mappers
{
    internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        private static readonly Dictionary<Type, HttpStatusCode> exceptionToHttpStatusCodeDictionary = new Dictionary<Type, HttpStatusCode>
        {
            { typeof(MethodNotAllowedException), HttpStatusCode.MethodNotAllowed },
            { typeof(InvalidRequestException), HttpStatusCode.NotImplemented }
        };

        public Response Map(Exception exception)
        {
            var exceptionType = exception.GetType();
            var statusCode = exceptionToHttpStatusCodeDictionary.ContainsKey(exceptionType)
                ? exceptionToHttpStatusCodeDictionary[exceptionType]
                : HttpStatusCode.InternalServerError;

            return new Response(statusCode, HeaderCollection.CreateForResponse());
        }
    }
}
