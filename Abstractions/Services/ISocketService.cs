using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abstractions.Services
{
    /// <summary>
    /// 定义socket通信服务的接口
    /// </summary>
    public interface ISocketService
    {
        /// <summary>
        /// 获取服务是否正在运行
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 获取或设置服务监听端口
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// 启动服务
        /// </summary>
        void Start();

        /// <summary>
        /// 停止服务
        /// </summary>
        void Stop();

        /// <summary>
        /// 异步启动服务
        /// </summary>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>表示异步操作的任务</returns>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步停止服务
        /// </summary>
        /// <param name="cancellationToken">取消标记</param>
        /// <returns>表示异步操作的任务</returns>
        Task StopAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 服务接收到消息时触发
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }

    /// <summary>
    /// 消息接收事件参数
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 接收到的消息
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 发送响应的回调
        /// </summary>
        public Action<string> RespondCallback { get; }

        public MessageReceivedEventArgs(string message, Action<string> respondCallback)
        {
            Message = message;
            RespondCallback = respondCallback;
        }
    }

}
