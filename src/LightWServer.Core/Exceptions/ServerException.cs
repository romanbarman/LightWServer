namespace LightWServer.Core.Exceptions
{
    internal class ServerException : Exception
    {
        internal ServerException(string message) : base(message) { }
    }
}
