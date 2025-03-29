using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abstractions.Services;

namespace Core.Services
{
    public class SocketService : ISocketService
    {
        private TcpListener _listener;
        private Thread _listenerThread;
        private bool _isRunning;
        private int _port = 8080;
        private CancellationTokenSource _cancellationTokenSource;

        public bool IsRunning => _isRunning;

        public int Port
        {
            get => _port;
            set => _port = value;
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void Start()
        {
            if (_isRunning) return;

            try
            {
                _isRunning = true;
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();

                _listenerThread = new Thread(ListenForClients)
                {
                    IsBackground = true
                };
                _listenerThread.Start();
            }
            catch (Exception ex)
            {
                _isRunning = false;
                throw new CommunicationException("Failed to start socket service", ex);
            }
        }

        public void Stop()
        {
            if (!_isRunning) return;

            try
            {
                _isRunning = false;

                _listener?.Stop();
                _listener = null;

                if (_listenerThread != null && _listenerThread.IsAlive)
                {
                    _listenerThread.Join(1000);
                }
            }
            catch (Exception ex)
            {
                throw new CommunicationException("Failed to stop socket service", ex);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (_isRunning) return;
            try
            {
                _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                _isRunning = true;
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();

                await Task.Run(() => ListenForClientsAsync(_cancellationTokenSource.Token), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // 操作被取消，正常退出
                _isRunning = false;
            }
            catch (Exception ex)
            {
                _isRunning = false;
                throw new CommunicationException("Failed to start socket service", ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            Stop();
            _cancellationTokenSource?.Cancel();
            return Task.CompletedTask;
        }

        private void ListenForClients()
        {
            try
            {
                while (_isRunning)
                {
                    TcpClient client = _listener.AcceptTcpClient();

                    Thread clientThread = new Thread(HandleClientCommunication)
                    {
                        IsBackground = true
                    };
                    clientThread.Start(client);
                }
            }
            catch (SocketException)
            {
                // 正常停止时会抛出此异常
            }
            catch (Exception ex)
            {
                // log
            }
        }

        private async Task ListenForClientsAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (_isRunning && !cancellationToken.IsCancellationRequested)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);

                    // 为每个客户端创建一个任务处理
                    _ = Task.Run(() => HandleClientCommunicationAsync(client, cancellationToken), cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // 正常取消
            }
            catch (SocketException)
            {
                // 正常停止时会抛出此异常
            }
            catch (Exception ex)
            {
                // log
            }
        }

        private void HandleClientCommunication(object clientObj)
        {
            TcpClient tcpClient = (TcpClient)clientObj;
            NetworkStream stream = tcpClient.GetStream();

            try
            {
                byte[] buffer = new byte[8192];

                while (_isRunning && tcpClient.Connected)
                {
                    // 读取客户端消息
                    int bytesRead = 0;

                    try
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                    }
                    catch (IOException)
                    {
                        // 客户端断开连接
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        // 客户端断开连接
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // 触发处理事件
                    void RespondCallback(string response)
                    {
                        if (!tcpClient.Connected) return;

                        byte[] responseData = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseData, 0, responseData.Length);
                    }

                    OnMessageReceived(message, RespondCallback);
                }
            }
            catch (Exception ex)
            {
                // log
            }
            finally
            {
                tcpClient.Close();
            }
        }

        private async Task HandleClientCommunicationAsync(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            using (tcpClient)
            {
                NetworkStream stream = tcpClient.GetStream();
                byte[] buffer = new byte[8192];
                try
                {
                    while (_isRunning && tcpClient.Connected && !cancellationToken.IsCancellationRequested)
                    {
                        int bytesRead = 0;
                        try
                        {
                            bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                        }
                        catch (IOException)
                        {
                            // 客户端断开连接
                            break;
                        }
                        if (bytesRead == 0)
                        {
                            // 客户端断开连接
                            break;
                        }
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        // 事件
                        void RespondCallback(string response)
                        {
                            if (!tcpClient.Connected) return;

                            byte[] responseData = Encoding.UTF8.GetBytes(response);
                            stream.WriteAsync(responseData, 0, responseData.Length, cancellationToken);
                        }

                        OnMessageReceived(message, RespondCallback);
                    }
                }
                catch (OperationCanceledException)
                {
                    // 正常取消
                }
                catch (Exception ex)
                {
                    // 处理异常，可以考虑添加日志
                }
            }
        }

        protected virtual void OnMessageReceived(string message, Action<string> respondCallback)
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(message, respondCallback));
        }
    }

    /// <summary>
    /// 通信服务异常
    /// </summary>
    public class CommunicationException : Exception
    {
        public CommunicationException(string message) : base(message) { }
        public CommunicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
