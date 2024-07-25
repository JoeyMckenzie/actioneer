using Actioneer;
using Actioneer.Simple.Actions;

var sayHelloAction = new SayHello("Gandalf");
var ping = new Ping();
var asyncPing = new PingAsync();

var dispatcher = new Dispatcher();
var asyncDispatcher = new AsyncDispatcher();
using var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(1));

dispatcher.Dispatch(sayHelloAction);
dispatcher.Dispatch(sayHelloAction);
// var ponged = dispatcher.Dispatch(ping);
// var awaitedPong = await asyncDispatcher.DispatchAsync(asyncPing, tokenSource.Token);
