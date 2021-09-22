using System;
using QuicServe;

HttpServer.Start();
Log.Info($"Web server on ports {HttpServer.PlainPort} and {HttpServer.SecurePort}");

Console.CancelKeyPress += delegate {
    HttpServer.Stop();
    Environment.Exit(0);
};

// just loop
Log.Debug("Press Ctrl+C to stop");
while (true) {
    var input = Console.ReadKey(intercept: true);
    if (input.Key == ConsoleKey.Enter) { Log.Empty(); }  // to add empty lines as needed
};
