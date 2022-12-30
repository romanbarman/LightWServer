namespace LightWServer.Core.Exceptions
{
    internal sealed class InvalidHeaderFormatException : ServerException
    {
        internal InvalidHeaderFormatException(string header) : base($"Invalid header format '{header}'.") {}
    }
}
