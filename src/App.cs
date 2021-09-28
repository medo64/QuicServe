using System;
using QuicServe;

HttpServer.Start();
Log.Info($"Web server on http://127.0.0.1:{Settings.PlainPort} and https://127.0.0.1:{Settings.SecurePort}");

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
