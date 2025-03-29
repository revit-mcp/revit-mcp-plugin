using System;
using Abstractions.Models.JsonRpc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Models.JsonRpc
{
    /// <summary>
    /// 表示JSON-RPC 2.0请求对象
    /// </summary>
    public class JsonRpcRequest: IJsonRpcRequest
    {
        /// <summary>
        /// JSON-RPC版本，必须为"2.0"
        /// </summary>
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; }

        /// <summary>
        /// 要调用的方法名称
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// 调用方法的参数
        /// 可以是对象或数组
        /// </summary>
        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public JToken Params { get; set; }

        /// <summary>
        /// 请求ID，用于匹配响应
        /// 对于通知请求，ID为null
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        /// <summary>
        /// 检查请求是否是通知
        /// 通知是没有ID的请求，不要求响应
        /// </summary>
        [JsonIgnore]
        public bool IsNotification => string.IsNullOrEmpty(Id);

        /// <summary>
        /// 验证请求是否有效
        /// </summary>
        /// <returns>如果请求有效则返回true，否则返回false</returns>
        public bool IsValid()
        {
            // jsonrpc必须为"2.0"
            if (JsonRpc != "2.0")
                return false;

            // method不能为空
            if (string.IsNullOrEmpty(Method))
                return false;

            // 通知请求id必须为null，普通请求id不能为null
            // 注意：当请求从JSON反序列化时，缺少的id属性会使Id为null，这是合法的通知请求

            return true;
        }

        /// <summary>
        /// 尝试获取参数为指定类型的对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="result">转换结果</param>
        /// <returns>如果转换成功则返回true，否则返回false</returns>
        public bool TryGetParamsAs<T>(out T result)
        {
            result = default;

            try
            {
                if (Params == null)
                    return false;

                result = Params.ToObject<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取参数为指定类型的对象
        /// 如果转换失败，则抛出异常
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>转换后的对象</returns>
        /// <exception cref="JsonRpcSerializationException">参数转换失败时抛出</exception>
        public T GetParamsAs<T>()
        {
            try
            {
                if (Params == null)
                {
                    throw new JsonRpcSerializationException("Request params is null");
                }

                return Params.ToObject<T>();
            }
            catch (Exception ex) when (!(ex is JsonRpcSerializationException))
            {
                throw new JsonRpcSerializationException(
                    $"Failed to convert params to type {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取参数值数组
        /// </summary>
        /// <returns>参数数组，如果参数不是数组则返回null</returns>
        public JArray GetParamsArray()
        {
            return Params as JArray;
        }

        /// <summary>
        /// 获取参数值对象
        /// </summary>
        /// <returns>参数对象，如果参数不是对象则返回null</returns>
        public JObject GetParamsObject()
        {
            return Params as JObject;
        }

        /// <summary>
        /// 尝试从数组参数中获取指定索引的参数值
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="index">参数索引</param>
        /// <param name="value">转换结果</param>
        /// <returns>如果获取成功则返回true，否则返回false</returns>
        public bool TryGetParamAt<T>(int index, out T value)
        {
            value = default;

            try
            {
                JArray array = GetParamsArray();
                if (array == null || index < 0 || index >= array.Count)
                    return false;

                value = array[index].ToObject<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 尝试从对象参数中获取指定属性的参数值
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">转换结果</param>
        /// <returns>如果获取成功则返回true，否则返回false</returns>
        public bool TryGetParam<T>(string propertyName, out T value)
        {
            value = default;

            try
            {
                JObject obj = GetParamsObject();
                if (obj == null || !obj.ContainsKey(propertyName))
                    return false;

                value = obj[propertyName].ToObject<T>();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建新的请求对象
        /// </summary>
        /// <param name="method">方法名称</param>
        /// <param name="parameters">方法参数</param>
        /// <param name="id">请求ID，如果为null则创建通知请求</param>
        /// <returns>创建的请求对象</returns>
        public static JsonRpcRequest Create(string method, object parameters = null, string id = null)
        {
            JToken paramsToken = parameters != null
                ? parameters is JToken token ? token : JToken.FromObject(parameters)
                : null;

            return new JsonRpcRequest
            {
                JsonRpc = "2.0",
                Method = method,
                Params = paramsToken,
                Id = id
            };
        }

        /// <summary>
        /// 创建一个通知请求
        /// 通知是不需要响应的请求
        /// </summary>
        /// <param name="method">方法名称</param>
        /// <param name="parameters">方法参数</param>
        /// <returns>创建的通知请求对象</returns>
        public static JsonRpcRequest CreateNotification(string method, object parameters = null)
        {
            return Create(method, parameters, null);
        }

        /// <summary>
        /// 将请求转换为JSON字符串
        /// </summary>
        /// <returns>序列化后的JSON字符串</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
