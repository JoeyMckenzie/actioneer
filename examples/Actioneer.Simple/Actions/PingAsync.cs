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

public class NotifyPongEffect : ISideEffect<PingAsync>
{
    public void Run(PingAsync action)
    {
        Console.WriteLine("Ponged... but sync!");
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
