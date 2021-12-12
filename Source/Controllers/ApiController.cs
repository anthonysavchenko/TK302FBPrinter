using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TK302FBPrinter.Business.Models;
using TK302FBPrinter.Business.Operations.Beep;
using TK302FBPrinter.Business.Operations.GetStatusOperation;
using TK302FBPrinter.Business.Operations.PrintReceipt;
using TK302FBPrinter.Business.Operations.PrintReportX;
using TK302FBPrinter.Business.Operations.PrintSlip;
using TK302FBPrinter.Business.Operations.PrintTicket;
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
        private readonly IPrintSlipOperation _printSlipOperation;
        private readonly IPrintReportXOperation _printReportXOperation;
        private readonly IGetStatusOperation _getStatusOperation;
        private readonly IPrintTicketOperation _printTicketOperation;

        public ApiController(
            IBeepOperation beepOperation,
            IShiftOpenOperation shiftOpenOperation,
            IShiftCloseOperation shiftCloseOperation,
            IPrintReceiptOperation printReceiptOperation,
            IPrintSlipOperation printSlipOperation,
            IPrintReportXOperation printReportXOperation,
            IGetStatusOperation getStatusOperation,
            IPrintTicketOperation printTicketOperation)
        {
            _beepOperation = beepOperation;
            _shiftOpenOperation = shiftOpenOperation;
            _shiftCloseOperation = shiftCloseOperation;
            _printReceiptOperation = printReceiptOperation;
            _printSlipOperation = printSlipOperation;
            _printReportXOperation = printReportXOperation;
            _getStatusOperation = getStatusOperation;
            _printTicketOperation = printTicketOperation;
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

        // POST /api/print/report/x
        [HttpPost("print/report/x")]
        public ActionResult<ExecutionResultDto> PrintReportX()
        {
            return Ok(new ExecutionResultDto(!_printReportXOperation.Execute()
                ? _printReportXOperation.ErrorDescriptions
                : null));
        }

        // POST /api/print/slip
        [HttpPost("print/slip")]
        public ActionResult<ExecutionResultDto> PrintSlip(SlipDto slipDto)
        {
            var slip = new Slip
            {
                Text = slipDto.Text
            };

            return Ok(new ExecutionResultDto(!_printSlipOperation.Execute(slip)
                ? _printSlipOperation.ErrorDescriptions
                : null));
        }

        // POST /api/print/receipt
        [HttpPost("print/receipt")]
        public ActionResult<ExecutionResultDto> PrintReceipt(ReceiptDto receiptDto)
        {
            var receipt = new Receipt
            {
                Tax = Enum.TryParse<TaxType>(receiptDto.Tax.ToString(), out TaxType tax) ? tax : TaxType.AutomaticMode,
                IsReturn = receiptDto.IsReturn,
                Total = receiptDto.Total,
                WithConnection = true,
                Items = receiptDto.Items
                    .Select(x =>
                        new ReceiptItem
                        {
                            Description = x.Description,
                            Quantity = x.Quantity,
                            Price = x.Price,
                            VAT = Enum.TryParse<VATType>(x.VAT.ToString(), out VATType vat) ? vat : VATType.NoVAT
                        })
                    .ToArray()
            };

            return Ok(new ExecutionResultDto(!_printReceiptOperation.Execute(receipt)
                ? _printReceiptOperation.ErrorDescriptions
                : null));
        }

        // POST /api/print/ticket
        [HttpPost("print/ticket")]
        public ActionResult<ExecutionResultDto> PrintTicket(TicketDto ticketDto)
        {
            var receipt = ticketDto.Receipt != null
                ? new Receipt
                {
                    Tax = Enum.TryParse<TaxType>(ticketDto.Receipt.Tax.ToString(), out TaxType tax)
                        ? tax
                        : TaxType.AutomaticMode,
                    IsReturn = ticketDto.Receipt.IsReturn,
                    Total = ticketDto.Receipt.Total,
                    WithConnection = false,
                    Items = ticketDto.Receipt.Items
                        .Select(x =>
                            new ReceiptItem
                            {
                                Description = x.Description,
                                Quantity = x.Quantity,
                                Price = x.Price,
                                VAT = Enum.TryParse<VATType>(x.VAT.ToString(), out VATType vat) ? vat : VATType.NoVAT
                            })
                        .ToArray()
                }
                : null;

            var ticket = new Ticket
            {
                TemplateName = ticketDto.TemplateName,
                Placeholders = ticketDto.Placeholders
                    .Select(x =>
                        new Placeholder
                        {
                            Key = x.Key,
                            Value = x.Value
                        })
                    .ToArray(),
                Seats = ticketDto.Seats
                    .Select(x =>
                        new Seat
                        {
                            Row = x.Row,
                            Place = x.Place
                        })
                    .ToArray(),
                SlipText = ticketDto.SlipText,
                Receipt = receipt
            };

            return Ok(new ExecutionResultDto(!_printTicketOperation.Execute(ticket)
                ? _printTicketOperation.ErrorDescriptions
                : null));
        }
    }
}
