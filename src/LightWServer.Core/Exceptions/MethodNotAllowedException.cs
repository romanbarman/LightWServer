namespace LightWServer.Core.Exceptions
{
    internal sealed class MethodNotAllowedException : ServerException
    {
        internal MethodNotAllowedException(string httpMethod) : base($"Not allowed '{httpMethod}' HTTP method.") { }
    }
}
