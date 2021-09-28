using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace QuicServe {
    internal static class Log {

        private static readonly object ConsoleSync = new();

        public static void Empty() {
            lock (ConsoleSync) {
                Console.WriteLine();
            }
        }

        public static void Debug(string text) {
            lock (ConsoleSync) {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        public static void Info(string text) {
            lock (ConsoleSync) {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        public static void Warning(string text) {
            lock (ConsoleSync) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        public static void Warning(string text, Exception exception) {
            lock (ConsoleSync) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(text + " (" + exception.Message + ")");
                Console.ResetColor();
            }
        }

        public static void Error(string text) {
            lock (ConsoleSync) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }


        public static void Startup() {
            var plainPort = Settings.PlainPort;
            var securePort = Settings.SecurePort;
            lock (ConsoleSync) {
                if ((plainPort > 0) && (securePort > 0)) {
                    Console.Write("Web server on ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"http://127.0.0.1:{plainPort}");
                    Console.ResetColor();
                    Console.Write(" and ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"https://127.0.0.1:{securePort}");
                    Console.ResetColor();
                } else if (plainPort > 0) {
                    Console.Write("Web server on ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"http://127.0.0.1:{plainPort}");
                    Console.ResetColor();
                } else if (securePort > 0) {
                    Console.Write("Web server on ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"https://127.0.0.1:{securePort}");
                    Console.ResetColor();
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Web server not listening");
                }
                Console.WriteLine();
            }
        }

        public static async Task Request(HttpRequest request) {
            var headerLines = new List<string>();
            foreach (var header in request.Headers) {
                if (!header.Key.StartsWith(":")) {  // ignore headers starting with colon
                    foreach (var value in header.Value) {
                        headerLines.Add($"{header.Key}: {value}");
                    }
                }
            }

            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var data = await reader.ReadToEndAsync().ConfigureAwait(false);

            if (!string.IsNullOrEmpty(data)) {
                var contentType = request.ContentType;
                if ((contentType is not null) && contentType.StartsWith("application/") && contentType.EndsWith("json")) {
                    try {
                        var jsonElement = JsonSerializer.Deserialize<JsonElement>(data);
                        data = JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions() { WriteIndented = true });
                    } catch (JsonException) { }
                }
            }

            lock (ConsoleSync) {
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(request.Protocol + " " + request.Method + " " + request.Path);
                foreach (var line in headerLines) {
                    Console.WriteLine(line);
                }

                Console.ForegroundColor = ConsoleColor.White;
                foreach (var line in data.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None)) {
                    Console.WriteLine(line);
                }

                Console.ResetColor();
            }
        }

    }
}
