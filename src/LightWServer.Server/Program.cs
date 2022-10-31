using LightWServer.Core;

var serverHost = new ServerBuilder().Build();
await serverHost.RunAsync();
