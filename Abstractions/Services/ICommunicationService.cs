using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions.Services
{
    /// <summary>
    /// 定义通信服务的接口
    /// </summary>
    public interface ICommunicationService
    {
        /// <summary>
        /// 初始化通信服务
        /// </summary>
        void Initialize();

        /// <summary>
        /// 启动通信服务
        /// </summary>
        void Start();

        /// <summary>
        /// 停止通信服务
        /// </summary>
        void Stop();

        /// <summary>
        /// 获取服务是否正在运行
        /// </summary>
        bool IsRunning { get; }
    }
}
