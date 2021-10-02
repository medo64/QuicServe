using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace QuicServe.Controllers {
    [ApiController]
    [Route("{**catchAll}")]
    public class CatchAllController : ControllerBase {

        public FileExtensionContentTypeProvider ContentTypeProvider = new();

        [HttpGet()]
        public async Task<IActionResult> Get() {
            await Log.Request(Request);
            var path = new PathSettings(Request.Path.Value);

            if (path.StatusCode != 0) {  // if status code is specified, use that
                return new HttpResult(path.StatusCode);
            } else {  // try getting the file
                if (!string.IsNullOrEmpty(path.FilePath)) {
                    var filePath = Helper.GetFullFilePath(path.FilePath);
                    if (filePath != null) {
                        if (System.IO.File.Exists(filePath)) {
                            if (!ContentTypeProvider.TryGetContentType(filePath, out var contentType)) {
                                contentType = "application/octet-stream";
                            }
                            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                            return new FileContentResult(bytes, contentType);
                        }
                    }
                }
            }
            return Ok();
        }

        [HttpDelete()]
        [HttpHead()]
        [HttpOptions()]
        [HttpPatch()]
        [HttpPost()]
        [HttpPut()]
        public async Task<IActionResult> Other() {
            await Log.Request(Request);
            return Ok();
        }

    }
}
