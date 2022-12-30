using LightWServer.Core.HttpContext;

namespace LightWServer.Core.Services.Mappers
{
    internal interface IExceptionToResponseMapper
    {
        Response Map(Exception exception);
    }
}
