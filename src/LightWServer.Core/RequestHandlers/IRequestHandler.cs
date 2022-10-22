using LightWServer.Core.HttpContext;

namespace LightWServer.Core.RequestHandlers
{
    internal interface IRequestHandler
    {
        Response Handle(Request request);
    }
}
