using System.Runtime.Serialization;

namespace LightWServer.Core.Exceptions
{
    [Serializable]
    internal sealed class MethodNotAllowedException : Exception
    {
        internal MethodNotAllowedException(string message) : base(message) { }
        internal MethodNotAllowedException(string message, Exception innerException) : base(message, innerException) { }
        internal MethodNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
