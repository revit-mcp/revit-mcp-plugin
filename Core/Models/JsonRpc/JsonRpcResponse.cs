using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Abstractions.Models.JsonRpc;

namespace Core.Models.JsonRpc
{
    /// <summary>
    /// JSON-RPC 2.0 成功响应
    /// </summary>
    public class JsonRpcSuccessResponse : IJsonRpcResponse
    {
        /// <summary>
        /// JSON-RPC版本
        /// </summary>
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; } = "2.0";

        /// <summary>
        /// 请求ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 响应结果
        /// </summary>
        [JsonProperty("result")]
        public JToken Result { get; set; }

        /// <summary>
        /// 将响应转换为JSON字符串
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// JSON-RPC 2.0 错误响应
    /// </summary>
    public class JsonRpcErrorResponse : IJsonRpcResponse
    {
        /// <summary>
        /// JSON-RPC版本
        /// </summary>
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; } = "2.0";

        /// <summary>
        /// 请求ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("error")]
        public JsonRpcError Error { get; set; }

        /// <summary>
        /// 将响应转换为JSON字符串
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// JSON-RPC 2.0 错误对象
    /// </summary>
    public class JsonRpcError : IJsonRpcError
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// 错误数据（可选）
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public JToken Data { get; set; }
    }
}
