using System;
using Microsoft.AspNetCore.Mvc;

namespace QuicServe {
    internal class HttpResult : ActionResult {

        public HttpResult(int statusCode) {
            StatusCode = statusCode;
        }


        public int StatusCode { get; }


        public override void ExecuteResult(ActionContext context) {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }

            var httpContext = context.HttpContext;
            httpContext.Response.StatusCode = StatusCode;
        }
    }
}
