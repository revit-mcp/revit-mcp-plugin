```
revit-mcp-plugin/
├── Abstractions/                          # 接口和抽象定义项目
│   ├── Commands/                          # 命令相关接口
│   │   ├── ICommand.cs                    # 基本命令接口
│   │   ├── IRevitCommand.cs               # Revit命令接口
│   │   ├── IExternalEventHandler.cs       # 外部事件处理器接口
│   │   └── IWaitableExternalEventHandler.cs # 可等待的外部事件处理器接口
│   │
│   ├── Services/                          # 服务相关接口
│   │   ├── ICommunicationService.cs       # 通信服务接口
│   │   ├── ISocketService.cs              # Socket服务接口
│   │   ├── IJsonRpcService.cs             # JSON-RPC服务接口
│   │   ├── IModuleService.cs              # 模块加载服务接口
│   │   └── IHotReloadService.cs           # 热重载服务接口
│   │
│   └── Models/                            # 数据模型接口和基类
│       ├── JsonRPC/                       # JSON-RPC模型
│       │   ├── IJsonRpcRequest.cs         # JSON-RPC请求接口
│       │   ├── IJsonRpcResponse.cs        # JSON-RPC响应接口
│       │   └── JsonRpcErrorCodes.cs       # 错误码常量
│       │
│       └── Configuration/                 # 配置模型
│           └── IPluginConfig.cs           # 插件配置接口
│
├── Core/                                  # 核心实现项目
│   ├── Services/                          # 服务实现
│   │   ├── Communication/                 # 通信相关服务
│   │   │   ├── SocketService.cs           # Socket服务实现
│   │   │   └── JsonRpcService.cs          # JSON-RPC服务实现
│   │   │
│   │   ├── Module/                        # 模块管理相关
│   │   │   ├── ModuleService.cs           # 模块加载服务
│   │   │   ├── ModuleLoader.cs            # 模块加载器
│   │   │   └── HotReloadService.cs        # 热重载服务
│   │   │
│   │   └── Serialization/                 # 序列化相关
│   │       └── JsonSerializer.cs          # JSON序列化服务
│   │
│   ├── Commands/                          # 命令处理
│   │   ├── Registry/                      # 命令注册
│   │   │   ├── CommandRegistry.cs         # 命令注册表
│   │   │   └── CommandRegistrar.cs        # 命令注册器
│   │   │
│   │   ├── Handlers/                      # 命令处理器
│   │   │   ├── CommandHandlerBase.cs      # 命令处理器基类
│   │   │   └── ExternalEventHandlerBase.cs # 外部事件处理器基类
│   │   │
│   │   └── Execution/                     # 命令执行
│   │       ├── CommandExecutor.cs         # 命令执行器
│   │       └── DynamicCommandExecutor.cs  # 动态命令执行器
│   │
│   └── Models/                            # 模型实现
│       ├── JsonRPC/                       # JSON-RPC模型实现
│       │   ├── JsonRpcRequest.cs          # JSON-RPC请求实现
│       │   ├── JsonRpcResponse.cs         # JSON-RPC响应实现
│       │   └── JsonRpcSerializer.cs       # JSON-RPC序列化器
│       │
│       └── Configuration/                 # 配置实现
│           └── PluginConfig.cs            # 插件配置实现
│
├── Host/                                  # Revit宿主项目
│   ├── App/                               # 应用程序相关
│   │   ├── RevitApplication.cs            # Revit应用程序入口
│   │   └── AppInitializer.cs              # 应用程序初始化
│   │
│   ├── Commands/                          # 外部命令
│   │   ├── MCPExternalCommand.cs          # MCP外部命令
│   │   └── ExternalCommandBase.cs         # 外部命令基类
│   │
│   └── Events/                            # 事件相关
│       ├── ExternalEventManager.cs        # 外部事件管理器
│       └── RevitEventAdapter.cs           # Revit事件适配器
│
└── ApiAdapter/                            # API适配器项目
    ├── Common/                            # 通用适配
    │   ├── RevitApiAdapterBase.cs         # Revit API适配器基类
    │   ├── IRevitApiAdapter.cs            # Revit API适配器接口
    │   └── RevitVersionDetector.cs        # Revit版本检测器
    │
    ├── v2020/                             # 2020版本适配
    │   └── Revit2020ApiAdapter.cs         # Revit 2020 API适配器
    │
    ├── v2021/                             # 2021版本适配
    │   └── Revit2021ApiAdapter.cs         # Revit 2021 API适配器
    │
    ├── v2022/                             # 2022版本适配
    │   └── Revit2022ApiAdapter.cs         # Revit 2022 API适配器
    │
    └── v2023/                             # 2023版本适配
        └── Revit2023ApiAdapter.cs         # Revit 2023 API适配器
```

## 说明

1. **Abstractions**
   - 包含所有接口定义，是底层架构的基础
   - 命令和服务接口确保核心功能与具体实现分离
   - 模型接口定义了数据交换格式
2. **Core**
   - 实现核心功能，但不直接依赖于Revit API
   - 命令注册表和执行器支持动态加载命令
   - 模块加载和热重载服务支持工具的动态更新
3. **Host**
   - 负责与Revit的集成
   - 管理外部命令和事件
   - 初始化和配置应用程序
4. **ApiAdapter**
   - 处理不同Revit版本之间的API差异
   - 提供统一的接口给Core层使用
   - 版本检测器自动选择适合当前Revit版本的适配器
