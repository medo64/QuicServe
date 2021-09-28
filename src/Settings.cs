using System;
using System.IO;
using Medo.Configuration;

namespace QuicServe {
    internal static class Settings {

        private static readonly object SyncRoot = new();
        private static IniFile? CachedConfig;

        private static IniFile Config() {
            lock (SyncRoot) {
                if (CachedConfig == null) {
                    var path = Path.Combine(AppContext.BaseDirectory, "QuicServe.ini");
                    CachedConfig = new IniFile(path);
                }
                return CachedConfig;
            }
        }


        public static int PlainPort {
            get {
                var port = Config().Read("", "PlainPort", 42080);
                if ((port < 1) || (port > 65535)) { port = 42080; }
                return port;
            }
        }

        public static int SecurePort {
            get {
                var port = Config().Read("", "SecurePort", 42080);
                if ((port < 1) || (port > 65535)) { port = 42080; }
                return port;
            }
        }

    }
}
