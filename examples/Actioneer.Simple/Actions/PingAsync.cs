using Actioneer.Core;

namespace Actioneer.Simple.Actions;

public class PingAsync : IAsyncDispatchable
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Pong");

        return Task.CompletedTask;
    }
}

public class NotifyPongAsyncEffect : IAsyncSideEffect<PingAsync>
{
    public Task RunAsync(PingAsync action, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Ping ponged just now!");

        return Task.CompletedTask;
    }
}

public class AnotherNotifyPongAsyncEffect : IAsyncSideEffect<PingAsync>
{
    public Task RunAsync(PingAsync action, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Ping ponged just now... just letting you know!");

        return Task.CompletedTask;
    }
}
