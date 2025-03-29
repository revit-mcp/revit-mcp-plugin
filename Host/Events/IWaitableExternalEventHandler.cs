using Abstractions.Commands;
using Autodesk.Revit.UI;

namespace Host.Events
{
    public interface IWaitableExternalEventHandler : IExternalEventHandler, IWaitableHandler
    {
    }
}
