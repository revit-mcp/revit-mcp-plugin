using Newtonsoft.Json.Linq;

namespace Abstractions.Models.JsonRpc
{
    public interface IJsonRpcRequest
    {
        /// <summary>
        /// JSON-RPC版本，必须为"2.0"
        /// </summary>
        string JsonRpc { get; set; }
        /// <summary>
        /// 要调用的方法名称
        /// </summary>
        string Method { get; set; }
        /// <summary>
        /// 调用方法的参数
        /// 可以是对象或数组
        /// </summary>
        JToken Params { get; set; }
        /// <summary>
        /// 请求ID，用于匹配响应
        /// 对于通知请求，ID为null
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// 检查请求是否是通知
        /// 通知是没有ID的请求，不要求响应
        /// </summary>
        bool IsNotification { get; }
        /// <summary>
        /// 验证请求是否有效
        /// </summary>
        /// <returns>如果请求有效则返回true，否则返回false</returns>
        bool IsValid();
        /// <summary>
        /// 尝试获取参数为指定类型的对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="result">转换结果</param>
        /// <returns>如果转换成功则返回true，否则返回false</returns>
        bool TryGetParamsAs<T>(out T result);
        /// <summary>
        /// 将请求转换为JSON字符串
        /// </summary>
        /// <returns>序列化后的JSON字符串</returns>
        string ToJson();

    }
}
