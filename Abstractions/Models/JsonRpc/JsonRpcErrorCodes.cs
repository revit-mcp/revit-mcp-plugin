namespace Abstractions.Models.JsonRpc
{
    /// <summary>
    /// JSON-RPC 2.0 错误代码定义
    /// 包含标准错误代码和自定义错误代码
    /// </summary>
    public static class JsonRpcErrorCodes
    {
        #region 标准JSON-RPC 2.0错误代码 (-32768 to -32000)

        /// <summary>
        /// 无效的JSON格式。
        /// 服务器接收到的不是有效的JSON。
        /// </summary>
        public const int ParseError = -32700;

        /// <summary>
        /// 无效的JSON-RPC请求。
        /// 发送的JSON不是有效的请求对象。
        /// </summary>
        public const int InvalidRequest = -32600;

        /// <summary>
        /// 请求的方法不存在或不可用。
        /// </summary>
        public const int MethodNotFound = -32601;

        /// <summary>
        /// 无效的方法参数。
        /// 方法参数无效或格式错误。
        /// </summary>
        public const int InvalidParams = -32602;

        /// <summary>
        /// 内部JSON-RPC错误。
        /// 处理请求时发生的通用服务器错误。
        /// </summary>
        public const int InternalError = -32603;

        /// <summary>
        /// 服务器错误的起始范围。
        /// 预留给具体实现定义的服务器错误。
        /// </summary>
        public const int ServerErrorStart = -32000;

        /// <summary>
        /// 服务器错误的结束范围。
        /// </summary>
        public const int ServerErrorEnd = -32099;

        #endregion

        #region Revit API相关错误代码 (-33000 to -33099)

        /// <summary>
        /// Revit API错误。
        /// 执行Revit API操作时发生错误。
        /// </summary>
        public const int RevitApiError = -33000;

        /// <summary>
        /// 命令执行超时。
        /// Revit命令执行时间超过预定的超时时间。
        /// </summary>
        public const int CommandExecutionTimeout = -33001;

        /// <summary>
        /// 当前文档不可用。
        /// 无法获取或访问当前Revit文档。
        /// </summary>
        public const int DocumentNotAvailable = -33002;

        /// <summary>
        /// 事务失败。
        /// Revit事务无法提交或回滚。
        /// </summary>
        public const int TransactionFailed = -33003;

        /// <summary>
        /// 元素不存在。
        /// 请求的Revit元素不存在或已被删除。
        /// </summary>
        public const int ElementNotFound = -33004;

        /// <summary>
        /// 元素创建失败。
        /// 无法创建新的Revit元素。
        /// </summary>
        public const int ElementCreationFailed = -33005;

        /// <summary>
        /// 元素修改失败。
        /// 无法修改现有的Revit元素。
        /// </summary>
        public const int ElementModificationFailed = -33006;

        /// <summary>
        /// 元素删除失败。
        /// 无法删除Revit元素。
        /// </summary>
        public const int ElementDeletionFailed = -33007;

        /// <summary>
        /// 无效的几何数据。
        /// 提供的几何数据无效或格式错误。
        /// </summary>
        public const int InvalidGeometryData = -33008;

        /// <summary>
        /// 视图不存在。
        /// 请求的Revit视图不存在。
        /// </summary>
        public const int ViewNotFound = -33009;

        #endregion

        #region 插件特定错误代码 (-33100 to -33199)

        /// <summary>
        /// 命令注册失败。
        /// 无法注册新命令。
        /// </summary>
        public const int CommandRegistrationFailed = -33100;

        /// <summary>
        /// 服务启动失败。
        /// 无法启动Socket服务。
        /// </summary>
        public const int ServiceStartupFailed = -33101;

        /// <summary>
        /// 无法创建外部事件。
        /// 创建Revit外部事件失败。
        /// </summary>
        public const int ExternalEventCreationFailed = -33102;

        /// <summary>
        /// 外部事件执行失败。
        /// Revit外部事件执行失败。
        /// </summary>
        public const int ExternalEventExecutionFailed = -33103;

        /// <summary>
        /// 命令取消。
        /// 命令被用户或系统取消。
        /// </summary>
        public const int CommandCancelled = -33104;

        /// <summary>
        /// 命令参数解析失败。
        /// 无法解析或转换命令参数。
        /// </summary>
        public const int CommandParameterParsingFailed = -33105;

        #endregion

        #region 一般应用错误代码 (-33200 to -33299)

        /// <summary>
        /// 未授权访问。
        /// 客户端没有执行请求操作的权限。
        /// </summary>
        public const int Unauthorized = -33200;

        /// <summary>
        /// 资源不可用。
        /// 请求的资源不可用或不存在。
        /// </summary>
        public const int ResourceUnavailable = -33201;

        /// <summary>
        /// 请求超时。
        /// 请求处理超时。
        /// </summary>
        public const int RequestTimeout = -33202;

        /// <summary>
        /// 无效的会话。
        /// 会话ID无效或已过期。
        /// </summary>
        public const int InvalidSession = -33203;

        /// <summary>
        /// 配置错误。
        /// 插件配置错误。
        /// </summary>
        public const int ConfigurationError = -33204;

        /// <summary>
        /// IO错误。
        /// 文件读写或网络IO错误。
        /// </summary>
        public const int IOError = -33205;

        #endregion

        /// <summary>
        /// 获取错误描述
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <returns>错误的描述文本</returns>
        public static string GetErrorDescription(int errorCode)
        {
            switch (errorCode)
            {
                // 标准JSON-RPC错误
                case ParseError: return "Invalid JSON was received by the server.";
                case InvalidRequest: return "The JSON sent is not a valid Request object.";
                case MethodNotFound: return "The method does not exist / is not available.";
                case InvalidParams: return "Invalid method parameter(s).";
                case InternalError: return "Internal JSON-RPC error.";

                // Revit API错误
                case RevitApiError: return "Revit API operation failed.";
                case CommandExecutionTimeout: return "Command execution timed out.";
                case DocumentNotAvailable: return "Revit document is not available.";
                case TransactionFailed: return "Revit transaction failed.";
                case ElementNotFound: return "Revit element not found.";
                case ElementCreationFailed: return "Failed to create Revit element.";
                case ElementModificationFailed: return "Failed to modify Revit element.";
                case ElementDeletionFailed: return "Failed to delete Revit element.";
                case InvalidGeometryData: return "Invalid geometry data.";
                case ViewNotFound: return "Revit view not found.";

                // 插件特定错误
                case CommandRegistrationFailed: return "Failed to register command.";
                case ServiceStartupFailed: return "Failed to start service.";
                case ExternalEventCreationFailed: return "Failed to create external event.";
                case ExternalEventExecutionFailed: return "External event execution failed.";
                case CommandCancelled: return "Command was cancelled.";
                case CommandParameterParsingFailed: return "Failed to parse command parameters.";

                // 一般应用错误
                case Unauthorized: return "Unauthorized access.";
                case ResourceUnavailable: return "Resource is unavailable.";
                case RequestTimeout: return "Request timed out.";
                case InvalidSession: return "Invalid session.";
                case ConfigurationError: return "Configuration error.";
                case IOError: return "I/O error.";

                // 服务器错误范围
                default:
                    if (errorCode >= ServerErrorStart && errorCode <= ServerErrorEnd)
                        return "Server error.";
                    return "Unknown error.";
            }
        }
    }
}
