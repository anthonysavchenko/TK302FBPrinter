using Microsoft.AspNetCore.Mvc;
using TK302FBPrinter.Device.Operations.Beep;
using TK302FBPrinter.Device.Operations.PrintReceipt;
using TK302FBPrinter.Device.Operations.ShiftClose;
using TK302FBPrinter.Device.Operations.ShiftOpen;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IBeepOperation _beepOperation;
        private readonly IShiftOpenOperation _shiftOpenOperation;
        private readonly IShiftCloseOperation _shiftCloseOperation;
        private readonly IPrintReceiptOperation _printReceiptOperation;

        public ApiController(
            IBeepOperation beepOperation,
            IShiftOpenOperation shiftOpenOperation,
            IShiftCloseOperation shiftCloseOperation,
            IPrintReceiptOperation printReceiptOperation)
        {
            _beepOperation = beepOperation;
            _shiftOpenOperation = shiftOpenOperation;
            _shiftCloseOperation = shiftCloseOperation;
            _printReceiptOperation = printReceiptOperation;
        }

        // POST /api/beep
        [HttpPost("beep")]
        public ActionResult<ExecutionResultDto> Beep()
        {
            return Ok(new ExecutionResultDto(!_beepOperation.Execute()
                ? _beepOperation.ErrorDescriptions
                : null));
        }

        // POST /api/shift/open
        [HttpPost("shift/open")]
        public ActionResult<ExecutionResultDto> ShiftOpen()
        {
            return Ok(new ExecutionResultDto(!_shiftOpenOperation.Execute()
                ? _shiftOpenOperation.ErrorDescriptions
                : null));
        }

        // POST /api/shift/close
        [HttpPost("shift/close")]
        public ActionResult<ExecutionResultDto> ShiftClose()
        {
            return Ok(new ExecutionResultDto(!_shiftCloseOperation.Execute()
                ? _shiftCloseOperation.ErrorDescriptions
                : null));
        }

        // POST /api/print/receipt
        [HttpPost("print/receipt")]
        public ActionResult<ExecutionResultDto> PrintReceipt(ReceiptDto receipt)
        {
            return Ok(new ExecutionResultDto(!_printReceiptOperation.Execute(receipt)
                ? _printReceiptOperation.ErrorDescriptions
                : null));
        }
    }
}
