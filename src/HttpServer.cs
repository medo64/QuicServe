using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace QuicServe {
    internal static class HttpServer {

        private static IHost? Builder;

        public static void Start() {
            Log.Debug("Starting HTTP server...");
            Builder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options => {
                        options.Listen(IPAddress.Any, PlainPort, listenOptions => {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                        });

                        var cert = GetCertificate();
                        options.Listen(IPAddress.Any, SecurePort, listenOptions => {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                            if (cert != null) {
                                listenOptions.UseHttps(cert);
                            } else {
                                listenOptions.UseHttps();
                            }
                        });
                    });
                })
                .Build();

            Task.Run(async delegate {
                try {
                    await Builder.StartAsync().ConfigureAwait(false);
                } catch (Exception ex) {
                    Log.Warning("Web server error", ex);
                }
            });
        }

        public static void Stop() {
            if (Builder != null) {
                Log.Debug("Stopping HTTP server...");
                Builder.StopAsync(TimeSpan.FromSeconds(3));
                Builder.Dispose();
                Builder = null;
            }
        }


        public static int PlainPort { get; } = 58080;
        public static int SecurePort { get; } = 58443;


        private static X509Certificate2? GetCertificate() {
            var certFilename = Path.Combine(AppContext.BaseDirectory, "quicserve.pfx");
            if (File.Exists(certFilename)) {
                try {
                    return new X509Certificate2(certFilename, "");
                } catch (CryptographicException ex) {
                    Log.Warning("Cannot load certificate; using default", ex);
                }
            }
            return null;
        }


        #region Startup

        private class Startup {

            public Startup(IConfiguration configuration) {
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

#pragma warning disable CA1822
            public void ConfigureServices(IServiceCollection services) {
#pragma warning restore CA1822
                services.AddControllers();

                services.AddLogging(builder => {
                    builder.ClearProviders();
                });
            }

#pragma warning disable CA1822
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
#pragma warning restore CA1822
#if DEBUG
                app.UseDeveloperExceptionPage();
#endif

                app.UseRouting();
                app.UseEndpoints(endpoints => {
                    endpoints.MapControllers();
                });
            }
        }

        #endregion Startup

    }
}
