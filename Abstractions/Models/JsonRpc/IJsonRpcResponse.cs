using Newtonsoft.Json.Linq;

namespace Abstractions.Models.JsonRpc
{
    /// <summary>
    /// JSON-RPC 2.0 标准响应接口
    /// </summary>
    public interface IJsonRpcResponse
    {
        /// <summary>
        /// JSON-RPC版本，始终为"2.0"
        /// </summary>
        string JsonRpc { get; }
        /// <summary>
        /// 请求ID，用于关联请求和响应
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// 将响应转换为JSON字符串
        /// </summary>
        /// <returns>序列化后的JSON字符串</returns>
        string ToJson();
    }
    /// <summary>
    /// JSON-RPC 2.0 错误对象接口
    /// </summary>
    public interface IJsonRpcError
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        int Code { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        string Message { get; set; }
        /// <summary>
        /// 错误数据（可选）
        /// </summary>
        JToken Data { get; set; }
    }
}
