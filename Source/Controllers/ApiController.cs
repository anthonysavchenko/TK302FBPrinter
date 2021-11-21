using Microsoft.AspNetCore.Mvc;
using TK302FBPrinter.Business.Operations.Beep;
using TK302FBPrinter.Business.Operations.GetStatusOperation;
using TK302FBPrinter.Business.Operations.PrintReceipt;
using TK302FBPrinter.Business.Operations.PrintReceiptReturn;
using TK302FBPrinter.Business.Operations.PrintReportX;
using TK302FBPrinter.Business.Operations.PrintSlip;
using TK302FBPrinter.Business.Operations.ShiftClose;
using TK302FBPrinter.Business.Operations.ShiftOpen;
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
        private readonly IPrintReceiptReturnOperation _printReceiptReturnOperation;
        private readonly IPrintSlipOperation _printSlipOperation;
        private readonly IPrintReportXOperation _printReportXOperation;
        private readonly IGetStatusOperation _getStatusOperation;

        public ApiController(
            IBeepOperation beepOperation,
            IShiftOpenOperation shiftOpenOperation,
            IShiftCloseOperation shiftCloseOperation,
            IPrintReceiptOperation printReceiptOperation,
            IPrintReceiptReturnOperation printReceiptReturnOperation,
            IPrintSlipOperation printSlipOperation,
            IPrintReportXOperation printReportXOperation,
            IGetStatusOperation getStatusOperation)
        {
            _beepOperation = beepOperation;
            _shiftOpenOperation = shiftOpenOperation;
            _shiftCloseOperation = shiftCloseOperation;
            _printReceiptOperation = printReceiptOperation;
            _printReceiptReturnOperation = printReceiptReturnOperation;
            _printSlipOperation = printSlipOperation;
            _printReportXOperation = printReportXOperation;
            _getStatusOperation = getStatusOperation;
        }

        // GET /api/status
        [HttpGet("status")]
        public ActionResult<PrinterStatusResultDto> Status()
        {
            return Ok(new PrinterStatusResultDto(
                !_getStatusOperation.Execute(out PrinterStatusDto status)
                    ? _getStatusOperation.ErrorDescriptions
                    : null,
                status));
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

        // POST /api/print/receipt/return
        [HttpPost("print/receipt/return")]
        public ActionResult<ExecutionResultDto> PrintReceiptReturn(ReceiptDto receipt)
        {
            return Ok(new ExecutionResultDto(!_printReceiptReturnOperation.Execute(receipt)
                ? _printReceiptReturnOperation.ErrorDescriptions
                : null));
        }

        // POST /api/print/slip
        [HttpPost("print/slip")]
        public ActionResult<ExecutionResultDto> PrintSlip(SlipContentDto content)
        {
            return Ok(new ExecutionResultDto(!_printSlipOperation.Execute(content.Text)
                ? _printSlipOperation.ErrorDescriptions
                : null));
        }

        // POST /api/print/report/x
        [HttpPost("print/report/x")]
        public ActionResult<ExecutionResultDto> PrintReportX()
        {
            return Ok(new ExecutionResultDto(!_printReportXOperation.Execute()
                ? _printReportXOperation.ErrorDescriptions
                : null));
        }
    }
}
