using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace QuicServe.Controllers {
    [ApiController]
    [Route("{**catchAll}")]
    public class CatchAllController : ControllerBase {

        [HttpDelete()]
        [HttpGet()]
        [HttpHead()]
        [HttpOptions()]
        [HttpPatch()]
        [HttpPost()]
        [HttpPut()]
        public async Task<IActionResult> CatchAll() {
            await Log.Request(Request);
            return Ok("");
        }

    }
}
