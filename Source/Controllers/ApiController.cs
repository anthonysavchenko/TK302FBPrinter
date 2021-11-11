using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Dto;
using TK302FBPrinter.Printer;

namespace TK302FBPrinter
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly PrinterOptions _printerOptions;
        private readonly IPrinterConnector _printerConnector;

        public ApiController(IOptionsSnapshot<PrinterOptions> printerOptions, IPrinterConnector printerConnector)
        {
            _printerOptions = printerOptions.Value;
            _printerConnector = printerConnector;
        }

        // POST /api/beep
        [HttpPost("beep")]
        public ActionResult<ExecutionResultDto> Beep()
        {
            return !_printerConnector.Beep(_printerOptions)
                ? Ok(new ExecutionResultDto(_printerConnector.GetErrorDescription()))
                : Ok(new ExecutionResultDto());
        }

        // POST /api/shift/open
        [HttpPost("shift/open")]
        public ActionResult<ExecutionResultDto> ShiftOpen()
        {
            return !_printerConnector.ShiftOpen(_printerOptions)
                ? Ok(new ExecutionResultDto(_printerConnector.GetErrorDescription()))
                : Ok(new ExecutionResultDto());
        }

        // POST /api/shift/close
        [HttpPost("shift/close")]
        public ActionResult<ExecutionResultDto> ShiftClose()
        {
            return !_printerConnector.ShiftClose(_printerOptions)
                ? Ok(new ExecutionResultDto(_printerConnector.GetErrorDescription()))
                : Ok(new ExecutionResultDto());
        }

        // POST /api/print/receipt
        [HttpPost("print/receipt")]
        public ActionResult<ExecutionResultDto> PrintReceipt(ReceiptDto receipt)
        {
            return !_printerConnector.PrintReceipt(_printerOptions, receipt)
                ? Ok(new ExecutionResultDto(_printerConnector.GetErrorDescription()))
                : Ok(new ExecutionResultDto());
        }
    }
}
