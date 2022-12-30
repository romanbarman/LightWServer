using LightWServer.Core.HttpContext;

namespace LightWServer.Core.Services
{
    internal interface IRequestReader
    {
        Task<Request> ReadAsync(Stream networkStream);
    }
}
