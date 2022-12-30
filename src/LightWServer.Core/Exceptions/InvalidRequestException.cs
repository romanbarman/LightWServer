namespace LightWServer.Core.Exceptions
{
    internal sealed class InvalidRequestException : ServerException
    {
        internal InvalidRequestException(string message) : base($"{message.TrimEnd('.')}.") { }
    }
}
