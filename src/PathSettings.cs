using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuicServe {
    internal class PathSettings {


        public PathSettings(string? urlPath) {
            RelativePath = urlPath?.TrimStart('/') ?? "";

            var config = Settings.Config();
            var sections = config.GetSections();
            foreach (var section in sections) {
                if (Helper.IsGlobMatch(section, RelativePath)) {
                    var statusCode = config.Read(section, "StatusCode", 0);
                    if ((statusCode < 100) || (statusCode > 599)) { statusCode = 0; }  // ignore status codes out of range
                    if (statusCode != 0) {
                        StatusCode = statusCode;
                    } else {
                        FilePath = config.Read(section, "Path", RelativePath);
                    }
                }
            }
        }


        public string RelativePath { get; }
        public int StatusCode { get; }
        public string? FilePath { get; }

        public bool IsStatusCodeInformational { get { return (StatusCode >= 100) && (StatusCode <= 199); } }
        public bool IsStatusCodeSuccessful { get { return (StatusCode >= 200) && (StatusCode <= 299); } }
        public bool IsStatusCodeRedirect { get { return (StatusCode >= 300) && (StatusCode <= 399); } }
        public bool IsStatusCodeClientError { get { return (StatusCode >= 400) && (StatusCode <= 499); } }
        public bool IsStatusCodeServerError { get { return (StatusCode >= 500) && (StatusCode <= 599); } }

    }
}
