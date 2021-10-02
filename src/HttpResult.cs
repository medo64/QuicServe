using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QuicServe {
    internal class HttpResult : ActionResult {

        public HttpResult(int statusCode, string? contentType = null, byte[]? content = null) {
            StatusCode = statusCode;
            ContentType = contentType;
            Content = content;
        }


        private int StatusCode { get; }
        private string? ContentType { get; }
        private byte[]? Content { get; }


        public override void ExecuteResult(ActionContext context) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            var httpContext = context.HttpContext;
            httpContext.Response.StatusCode = StatusCode;

            if ((Content is not null) && (Content.Length > 0)) {
                httpContext.Response.ContentType = ContentType ?? "application/octet-stream";
                Task.Run(async () => {
                    await httpContext.Response.Body.WriteAsync(Content).ConfigureAwait(false);
                }).Wait();
            }
        }
    }
}
