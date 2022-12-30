namespace LightWServer.Core.Exceptions
{
    internal sealed class InvalidHttpVersionFormatException : ServerException
    {
        internal InvalidHttpVersionFormatException(string httpVersion) : base($"Invalid HTTP version format '{httpVersion}'.") { }
    }
}
