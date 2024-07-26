using Actioneer;
using Actioneer.Simple.Actions;

var sayHelloAction = new SayHello("Gandalf");
var ping = new Ping();
var asyncPing = new PingAsync();

var actioneer = new Actioneer.Actioneer();
using var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(120));

actioneer.Dispatch(sayHelloAction);

// dispatcher.Dispatch(sayHelloAction);
// var ponged = dispatcher.Dispatch(ping);
// var pongeds = dispatcher.Dispatch(ping);
await actioneer.DispatchAsync(asyncPing, tokenSource.Token);
