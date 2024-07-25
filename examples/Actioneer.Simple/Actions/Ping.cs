using Actioneer.Core;

namespace Actioneer.Simple.Actions;

public class Ping : IDispatchable<string>
{
    public string Execute()
    {
        return "Pong";
    }
}
