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
                    var iniPath = Helper.GetFilePath("QuicServe.ini");
                    if (iniPath != null) {
                        try {
                            CachedConfig = new IniFile(Path.Combine(AppContext.BaseDirectory, iniPath));
                        } catch(IOException) { }
                    }
                    if (CachedConfig == null) { CachedConfig = new IniFile(); }  // just give it an empty file
                }
                return CachedConfig;
            }
        }


        public static int PlainPort {
            get {
                var port = Config().Read("", "PlainPort", 42080);
                if ((port < 0) || (port > 65535)) { port = 42080; }
                return port;
            }
        }

        public static int SecurePort {
            get {
                var port = Config().Read("", "SecurePort", 42443);
                if ((port < 0) || (port > 65535)) { port = 42443; }
                return port;
            }
        }

    }
}
