using System;

namespace Core.Models.JsonRpc
{
    /// <summary>
    /// JSON-RPC序列化异常
    /// </summary>
    public class JsonRpcSerializationException : Exception
    {
        public JsonRpcSerializationException(string message) : base(message)
        {
        }

        public JsonRpcSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
