namespace LightWServer.Core.Exceptions
{
    internal sealed class HeaderNotExistException : ServerException
    {
        internal HeaderNotExistException(string headerName) : base($"Header '{headerName}' does not exist.") { }
    }
}
