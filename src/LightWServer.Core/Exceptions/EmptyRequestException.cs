namespace LightWServer.Core.Exceptions
{
    internal sealed class EmptyRequestException : ServerException
    {
        internal EmptyRequestException() : base("Request is empty.") { }
    }
}
