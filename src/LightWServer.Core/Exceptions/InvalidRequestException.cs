using System.Runtime.Serialization;

namespace LightWServer.Core.Exceptions
{
    [Serializable]
    internal sealed class InvalidRequestException : Exception
    {
        public InvalidRequestException(string message) : base(message) { }
        public InvalidRequestException(string message, Exception innerException) : base(message, innerException) { }
        public InvalidRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
