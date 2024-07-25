using Actioneer.Core;

namespace Actioneer.Simple.Actions;

public class SayHello(string name) : IDispatchable
{
    public string Name { get; } = name;

    public void Execute()
    {
        Console.WriteLine($"Hello {Name}!");
    }
}

public class SayHelloSideEffect : ISideEffect<SayHello>
{
    public void Run(SayHello action)
    {
        Console.WriteLine($"Hello again {action.Name}!");
    }
}
