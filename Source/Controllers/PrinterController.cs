using Microsoft.AspNetCore.Mvc;

namespace TK302FBPrinter
{
    [Route("[controller]")]
    [ApiController]
    public class PrinterController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetStatus()
        {
            return NoContent();
        }
    }
}
