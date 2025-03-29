using Abstractions.Services;
using Abstractions.Models.JsonRpc;
using Core.Models.JsonRpc;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Services.Communication
{
    /// <summary>
    /// JSON-RPC 服务实现
    /// </summary>
    public class JsonRpcService : IJsonRpcService
    {
        private readonly ISocketService _socketService;
        private readonly RevitCommandRegistry _commandRegistry;
        private bool _isInitialized = false;

        public JsonRpcService(ISocketService socketService,RevitCommandRegistry commandRegistry)
        {
            _socketService = socketService ?? throw new ArgumentNullException(nameof(socketService));
            _commandRegistry = commandRegistry ?? throw new ArgumentNullException(nameof(commandRegistry));
        }

        public bool IsRunning => _socketService.IsRunning;

        public void Initialize()
        {
            if (_isInitialized) return;

            // 订阅消息接收事件
            _socketService.MessageReceived += HandleSocketMessage;
            _isInitialized = true;
        }

        public void Start()
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            _socketService.Start();
        }

        public void Stop()
        {
            _socketService.Stop();
        }

        private void HandleSocketMessage(object sender, MessageReceivedEventArgs e)
        {
            string response = ProcessJsonRPCRequest(e.Message);
            e.RespondCallback(response);
        }

        private string ProcessJsonRPCRequest(string requestJson)
        {
            JsonRpcRequest request;
            try
            {
                // 解析JSON-RPC请求
                request = JsonConvert.DeserializeObject<JsonRpcRequest>(requestJson);
                // 验证请求格式是否有效
                if (request == null || !request.IsValid())
                {
                    return CreateErrorResponse(
                        null,
                        JsonRpcErrorCodes.InvalidRequest,
                        "Invalid JSON-RPC request"
                    );
                }
                // 查找命令
                if (!_commandRegistry.TryGetCommand(request.Method, out var command))
                {
                    return CreateErrorResponse(request.Id, JsonRpcErrorCodes.MethodNotFound,
                        $"Method '{request.Method}' not found");
                }
                // 执行命令
                try
                {
                    object result = command.Execute(request.GetParamsObject(), request.Id);
                    return CreateSuccessResponse(request.Id, result);
                }
                catch (Exception ex)
                {
                    return CreateErrorResponse(request.Id, JsonRpcErrorCodes.InternalError, ex.Message);
                }
            }
            catch (JsonException)
            {
                // JSON解析错误
                return CreateErrorResponse(
                    null,
                    JsonRpcErrorCodes.ParseError,
                    "Invalid JSON"
                );
            }
            catch (Exception ex)
            {
                // 处理请求时的其他错误
                return CreateErrorResponse(
                    null,
                    JsonRpcErrorCodes.InternalError,
                    $"Internal error: {ex.Message}"
                );
            }
        }
        private string CreateSuccessResponse(string id, object result)
        {
            var response = new JsonRpcSuccessResponse
            {
                Id = id,
                Result = result is JToken jToken ? jToken : JToken.FromObject(result)
            };
            return response.ToJson();
        }
        private string CreateErrorResponse(string id, int code, string message, object data = null)
        {
            var response = new JsonRpcErrorResponse
            {
                Id = id,
                Error = new JsonRpcError
                {
                    Code = code,
                    Message = message,
                    Data = data != null ? JToken.FromObject(data) : null
                }
            };
            return response.ToJson();
        }
    }
}
