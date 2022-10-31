using System.Runtime.Serialization;

namespace LightWServer.Core.Exceptions
{
    [Serializable]
    internal sealed class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException(string message) : base(message) { }
        public MethodNotAllowedException(string message, Exception innerException) : base(message, innerException) { }
        public MethodNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
