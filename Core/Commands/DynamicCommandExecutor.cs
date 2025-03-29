using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Commands
{
    public class DynamicCommandExecutor: IExternalEventHandler, IWaitableExternalEventHandler
    {
        private Action<UIApplication> _currentAction;
        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private Exception _exception;

        public bool TaskCompleted { get; private set; }
    }
}
