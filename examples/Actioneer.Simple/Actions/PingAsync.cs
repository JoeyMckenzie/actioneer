using Actioneer.Core;

namespace Actioneer.Simple.Actions;

public class PingAsync : IAsyncDispatchable<string>
{
    public Task<string> Execute()
    {
        return Task.FromResult("Pong");
    }
}
