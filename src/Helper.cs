using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace QuicServe {
    internal static class Helper {

        public static string? GetFullFilePath(string relativeFilePath) {  // gets case-insensitive path from relative path
            var pathParts = new Queue<string>(relativeFilePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));

            var currDirectory = new DirectoryInfo(AppContext.BaseDirectory);
            while (pathParts.Count > 0) {
                var pathPart = pathParts.Dequeue();

                DirectoryInfo? directoryCandidate = null;
                foreach (var directory in currDirectory.EnumerateDirectories()) {
                    if (directory.Name.Equals(pathPart, StringComparison.InvariantCulture)) {
                        directoryCandidate = directory;
                        break;  // if exact match is found, we're done
                    } else if (directory.Name.Equals(pathPart, StringComparison.InvariantCultureIgnoreCase)) {
                        directoryCandidate = directory;
                    }
                }

                if (directoryCandidate != null) {  // directory found
                    currDirectory = directoryCandidate;
                } else if (pathParts.Count == 0) {  // this might be a file entry
                    FileInfo? fileCandidate = null;
                    foreach (var file in currDirectory.EnumerateFiles()) {
                        if (file.Name.Equals(pathPart, StringComparison.InvariantCulture)) {
                            fileCandidate = file;
                            break;  // if exact match is found, we're done
                        } else if (file.Name.Equals(pathPart, StringComparison.InvariantCultureIgnoreCase)) {
                            fileCandidate = file;
                        }
                    }
                    if (fileCandidate != null) {
                        return fileCandidate.FullName;
                    } else {
                        break;  // file not found :(
                    }
                } else {
                    break;  // part of path found :(
                }
            }

            return null;
        }

        private static readonly Dictionary<string, Regex> GlobRegexCache = new();
        public static bool IsGlobMatch(string glob, string text) {
            if (glob == "") { return false; }

            if (!GlobRegexCache.TryGetValue(glob, out var regex)) {
                var pattern = "^" + Regex.Escape(glob).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
                regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
                GlobRegexCache.Add(glob, regex);
            }
            return regex.IsMatch(text);
        }

    }
}
