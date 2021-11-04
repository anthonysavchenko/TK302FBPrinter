using Microsoft.AspNetCore.Mvc;
using TK302FBPrinter.Printer;

namespace TK302FBPrinter
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IPrinterConnector _printerConnector;

        public ApiController(IPrinterConnector printerConnector)
        {
            _printerConnector = printerConnector;
        }

        // POST /api/beep
        [HttpPost("beep")]
        public ActionResult Beep()
        {
            if (_printerConnector.Beep())
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
