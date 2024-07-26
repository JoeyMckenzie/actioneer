using Actioneer;
using Actioneer.Simple.Actions;

var sayHelloAction = new SayHello("Gandalf");
var ping = new Ping();
var asyncPing = new PingAsync();

var dispatcher = new ActioneerDispatcher();
using var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(120));

dispatcher.Dispatch(sayHelloAction);
var ponged = dispatcher.Dispatch(ping);
await dispatcher.DispatchAsync(asyncPing, tokenSource.Token);
