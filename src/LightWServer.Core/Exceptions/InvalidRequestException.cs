using System.Runtime.Serialization;

namespace LightWServer.Core.Exceptions
{
    [Serializable]
    internal sealed class InvalidRequestException : Exception
    {
        internal InvalidRequestException(string message) : base(message) { }
        internal InvalidRequestException(string message, Exception innerException) : base(message, innerException) { }
        internal InvalidRequestException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
