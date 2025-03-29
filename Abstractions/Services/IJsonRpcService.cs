using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions.Services
{
    /// <summary>
    /// 定义JSON-RPC服务的接口
    /// </summary>
    public interface IJsonRpcService : ICommunicationService
    {
        // 目前继承自ICommunicationService即可，后续可以根据需要添加其他方法
    }
}
