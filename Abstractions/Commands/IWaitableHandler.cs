using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstractions.Commands
{
    public interface IWaitableHandler
    {
        bool WaitForCompletion(int timeoutMs);
    }
}
