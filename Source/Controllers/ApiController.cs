using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TK302FBPrinter.Business.Models;
using TK302FBPrinter.Business.Operations.Beep;
using TK302FBPrinter.Business.Operations.GetStatusOperation;
using TK302FBPrinter.Business.Operations.PrintComplexDoc;
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
        private readonly IPrintComplexDocOperation _printComplexDocOperation;

        public ApiController(
            IBeepOperation beepOperation,
            IShiftOpenOperation shiftOpenOperation,
            IShiftCloseOperation shiftCloseOperation,
            IPrintReceiptOperation printReceiptOperation,
            IPrintSlipOperation printSlipOperation,
            IPrintReportXOperation printReportXOperation,
            IGetStatusOperation getStatusOperation,
            IPrintTicketOperation printTicketOperation,
            IPrintComplexDocOperation printComplexDocOperation)
        {
            _beepOperation = beepOperation;
            _shiftOpenOperation = shiftOpenOperation;
            _shiftCloseOperation = shiftCloseOperation;
            _printReceiptOperation = printReceiptOperation;
            _printSlipOperation = printSlipOperation;
            _printReportXOperation = printReportXOperation;
            _getStatusOperation = getStatusOperation;
            _printTicketOperation = printTicketOperation;
            _printComplexDocOperation = printComplexDocOperation;
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
            var ticket = new Ticket
            {
                TemplateName = ticketDto.TemplateName,
                WithConnection = true,
                Cut = true,
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
                    .ToArray()
            };

            return Ok(new ExecutionResultDto(!_printTicketOperation.Execute(ticket)
                ? _printTicketOperation.ErrorDescriptions
                : null));
        }

        // POST /api/print/complex-doc
        // POST /api/print
        [HttpPost("print/complex-doc")]
        [HttpPost("print")]
        public ActionResult<ExecutionResultDto> PrintComplexDoc(ComplexDocDto complexDocDto)
        {
            var complexDoc = new ComplexDoc
            {
                Slip = !string.IsNullOrEmpty(complexDocDto.SlipCheck)
                    ? new ComplexDocSlip
                    {
                        Text = complexDocDto.SlipCheck,
                        Cut = complexDocDto.Tickets == null && (complexDocDto.Goods == null || complexDocDto.Reprint)
                    }
                    : null,
                Ticket = complexDocDto.Tickets != null
                    ? new Ticket
                    {
                        TemplateName = "Template1",
                        WithConnection = false,
                        Cut = complexDocDto.Goods == null || complexDocDto.Reprint,
                        Placeholders = MapPlaceholders(complexDocDto.Tickets),
                        Seats = complexDocDto.Tickets.Seats
                            .Select(x =>
                                new Seat
                                {
                                    Row = x.Row,
                                    Place = x.Place
                                })
                            .ToArray()
                    }
                    : null,
                Receipt = complexDocDto.Goods != null && !complexDocDto.Reprint
                    ? new Receipt
                    {
                        Tax = Enum.TryParse<TaxType>(complexDocDto.Goods.Tax.ToString(), out TaxType tax)
                            ? tax
                            : TaxType.AutomaticMode,
                        IsReturn = false,
                        Total = complexDocDto.Goods.Amount,
                        WithConnection = false,
                        Items = complexDocDto.Goods.Items
                            .Select(x =>
                                new ReceiptItem
                                {
                                    Description = x.Name,
                                    Quantity = x.Count,
                                    Price = x.Price,
                                    VAT = VATType.Percent0
                                })
                            .ToArray()
                    }
                    : null
            };

            return Ok(new ExecutionResultDto(!_printComplexDocOperation.Execute(complexDoc)
                ? _printComplexDocOperation.ErrorDescriptions
                : null));
        }

        private Placeholder[] MapPlaceholders(ComplexDocTicketsDto complexDocTicketsDto)
        {
            var placeholders = new Placeholder[] {};

            if (complexDocTicketsDto == null)
            {
                return placeholders;
            }

            var placeholderList = new List<Placeholder>()
            {
                new Placeholder
                {
                    Key = "theater_name",
                    Value = complexDocTicketsDto.TheatreName
                },
                new Placeholder
                {
                    Key = "theater_legal_name",
                    Value = complexDocTicketsDto.TheatreLegalName
                },
                new Placeholder
                {
                    Key = "ogrn",
                    Value = complexDocTicketsDto.OGRN
                },
                new Placeholder
                {
                    Key = "inn",
                    Value = complexDocTicketsDto.INN
                },
                new Placeholder
                {
                    Key = "legal_address",
                    Value = complexDocTicketsDto.LegalAddress
                },
                new Placeholder
                {
                    Key = "movie",
                    Value = complexDocTicketsDto.Movie
                },
                new Placeholder
                {
                    Key = "format",
                    Value = complexDocTicketsDto.Format
                },
                new Placeholder
                {
                    Key = "license",
                    Value = complexDocTicketsDto.License
                },
                new Placeholder
                {
                    Key = "age",
                    Value = complexDocTicketsDto.Age
                },
                new Placeholder
                {
                    Key = "show_date",
                    Value = complexDocTicketsDto.ShowDate
                },
                new Placeholder
                {
                    Key = "hall",
                    Value = complexDocTicketsDto.Hall
                },
                new Placeholder
                {
                    Key = "amount",
                    Value = complexDocTicketsDto.Amount.ToString()
                },
                new Placeholder
                {
                    Key = "print_code",
                    Value = complexDocTicketsDto.PrintCode
                },
                new Placeholder
                {
                    Key = "payment_type",
                    Value = Enum.GetName(typeof(PaymentTypeDto), complexDocTicketsDto.PaymentType).ToLower()
                },
                new Placeholder
                {
                    Key = "payment_date",
                    Value = complexDocTicketsDto.PaymentDate
                },
                new Placeholder
                {
                    Key = "order_id",
                    Value = complexDocTicketsDto.OrderId
                }
            };
            
            if (complexDocTicketsDto.Discount != null)
            {
                placeholderList.Add(new Placeholder
                {
                    Key = "discount",
                    Value = complexDocTicketsDto.Discount.ToString()
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.Certificate))
            {
                placeholderList.Add(new Placeholder
                {
                    Key = "certificate",
                    Value = complexDocTicketsDto.Certificate
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.BonusCard))
            {
                placeholderList.Add(new Placeholder
                {
                    Key = "bonus_card",
                    Value = complexDocTicketsDto.BonusCard
                });
            }

            if (complexDocTicketsDto.BonusType != null)
            {
                placeholderList.Add(new Placeholder
                {
                    Key = "bonus_type",
                    Value = Enum.GetName(typeof(BonusTypeDto), complexDocTicketsDto.BonusType).ToLower()
                });
            }

            if (complexDocTicketsDto.PaymentType != null)
            {
                placeholderList.Add(new Placeholder
                {
                    Key = "payment_type",
                    Value = Enum.GetName(typeof(PaymentTypeDto), complexDocTicketsDto.PaymentType).ToLower()
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.Cashier))
            {
                placeholderList.Add(new Placeholder
                {
                    Key = "cashier",
                    Value = complexDocTicketsDto.Cashier
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.Tax))
            {
                placeholderList.Add(new Placeholder
                {
                    Key = "tax",
                    Value = complexDocTicketsDto.Tax
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.Comment))
            {
                placeholderList.Add(new Placeholder
                {
                    Key = "comment",
                    Value = complexDocTicketsDto.Comment
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.Email))
            {
                placeholderList.Add(new Placeholder
                {
                    Key = "email",
                    Value = complexDocTicketsDto.Email
                });
            }

            return placeholderList.ToArray();            
        }
    }
}
