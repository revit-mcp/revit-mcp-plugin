using Newtonsoft.Json.Linq;

namespace Abstractions.Commands
{
    public interface IRevitCommand
    {
        string CommandName { get; }
        object Execute(JObject parameters, string requestId);
    }
}
