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
                Tax =
                    Enum.TryParse<TaxType>(
                        Enum.GetName(typeof(TaxTypeDto), receiptDto.Tax),
                        out TaxType tax)
                    ? tax
                    : TaxType.AutomaticMode,
                IsReturn = receiptDto.IsReturn,
                Supplier = receiptDto.Supplier != null
                    ? new Supplier
                    {
                        INN = receiptDto.Supplier.INN,
                        CompanyName = receiptDto.Supplier.CompanyName,
                        Phone = receiptDto.Supplier.Phone
                    }
                    : null,
                Total = receiptDto.Total,
                WithConnection = true,
                Cut = true,
                Items = receiptDto.Items
                    .Select(x =>
                        new ReceiptItem
                        {
                            Description = x.Description,
                            Quantity = x.Quantity,
                            Price = x.Price,
                            VAT =
                                Enum.TryParse<VATType>(
                                    Enum.GetName(typeof(VATTypeDto), x.VAT),
                                    out VATType vat)
                                ? vat
                                : VATType.NoVAT
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
        [HttpPost("print/complex-doc")]
        public ActionResult<ComplexDocExecutionResultDto> PrintComplexDoc(ComplexDocDto complexDocDto)
        {
            // Кассовый чек для билетов содержит только те позиции, название которых равно "Кинобилет"
            var ticketItems = complexDocDto.Goods != null
                ? complexDocDto.Goods.Items
                    .Where(x => x.Name == "Кинобилет")
                    .Select(x =>
                        new ReceiptItem
                        {
                            Description = x.Name,
                            Quantity = x.Count,
                            Price = x.Price,
                            VAT = MapVATType(complexDocDto.Tickets.NDS)
                        })
                    .ToArray()
                : null;

            ticketItems = ticketItems != null && ticketItems.Length == 0 ? null : ticketItems;

            // Кассовый чек для товаров содержит только те позиции, название которых НЕ равно "Кинобилет"
            var goodsItems = complexDocDto.Goods != null
                ? complexDocDto.Goods.Items
                    .Where(x => x.Name != "Кинобилет")
                    .Select(x =>
                        new ReceiptItem
                        {
                            Description = x.Name,
                            Quantity = x.Count,
                            Price = x.Price,
                            VAT = MapVATType(complexDocDto.Goods.NDS)
                        })
                    .ToArray()
                : null;

            goodsItems = goodsItems != null && goodsItems.Length == 0 ? null : goodsItems;

            var complexDoc = new ComplexDoc
            {
                Ticket = complexDocDto.Tickets != null
                    ? new Ticket
                    {
                        TemplateName = "Template1",
                        WithConnection = false,
                        Cut =
                            (string.IsNullOrEmpty(complexDocDto.SlipCheck) && ticketItems == null && goodsItems == null)
                            || complexDocDto.Reprint,
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
                Slip = !string.IsNullOrEmpty(complexDocDto.SlipCheck) && !complexDocDto.Reprint
                    ? new ComplexDocSlip
                    {
                        Text = complexDocDto.SlipCheck,
                        Cut = ticketItems == null && goodsItems == null
                    }
                    : null,
                TicketReceipt = ticketItems != null && !complexDocDto.Reprint
                    ? new Receipt
                    {
                        Tax =
                            Enum.TryParse<TaxType>(
                                Enum.GetName(typeof(ComplexDocTaxTypeDto), complexDocDto.Tickets.Tax),
                                out TaxType ticketTax)
                            ? ticketTax
                            : TaxType.AutomaticMode,
                        IsReturn = false,
                        Supplier = complexDocDto.Tickets.Agent
                            ? new Supplier
                            {
                                INN = complexDocDto.Tickets.INN,
                                CompanyName = complexDocDto.Tickets.AgentName,
                                Phone = complexDocDto.Tickets.AgentPhone
                            }
                            : null,
                        Total = complexDocDto.Tickets.Amount,
                        WithConnection = false,
                        Cut = goodsItems == null,
                        Items = ticketItems
                    }
                    : null,
                GoodsReceipt = goodsItems != null && !complexDocDto.Reprint
                    ? new Receipt
                    {
                        Tax =
                            Enum.TryParse<TaxType>(
                                Enum.GetName(typeof(ComplexDocTaxTypeDto), complexDocDto.Goods.Tax),
                                out TaxType goodsTax)
                            ? goodsTax
                            : TaxType.AutomaticMode,
                        IsReturn = false,
                        Supplier = complexDocDto.Goods.Agent
                            ? new Supplier
                            {
                                INN = complexDocDto.Goods.INN,
                                CompanyName = complexDocDto.Goods.AgentName,
                                Phone = complexDocDto.Goods.AgentPhone
                            }
                            : null,
                        Total = complexDocDto.Goods.Amount,
                        WithConnection = false,
                        Cut = true,
                        Items = goodsItems
                    }
                    : null
            };

            return Ok(new ComplexDocExecutionResultDto(!_printComplexDocOperation.Execute(complexDoc)
                ? _printComplexDocOperation.ErrorDescriptions
                : null));
        }

        // Возвращает в виде массива список плейсхолдеров и строк для их замены
        private Placeholder[] MapPlaceholders(ComplexDocTicketsDto complexDocTicketsDto)
        {
            if (complexDocTicketsDto == null)
            {
                return new Placeholder[] {};
            }

            var placeholders = new List<Placeholder>()
            {
                // Далее добавляются обязательные плейсхолдеры, которые всегда передаются в запросе
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
                },
                new Placeholder
                {
                    Key = "viewers_count",
                    Value = complexDocTicketsDto.ViewersCount.ToString()
                }
            };
            
            // Далее добавляются необязательные плейсхолдеры, если они переданы в запросе
            
            if (complexDocTicketsDto.Discount != null)
            {
                placeholders.Add(new Placeholder
                {
                    Key = "discount",
                    Value = complexDocTicketsDto.Discount.ToString()
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.Certificate))
            {
                placeholders.Add(new Placeholder
                {
                    Key = "certificate",
                    Value = complexDocTicketsDto.Certificate
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.BonusCard))
            {
                placeholders.Add(new Placeholder
                {
                    Key = "bonus_card",
                    Value = complexDocTicketsDto.BonusCard
                });
            }

            if (complexDocTicketsDto.BonusType != null)
            {
                placeholders.Add(new Placeholder
                {
                    Key = "bonus_type",
                    Value = Enum.GetName(typeof(BonusTypeDto), complexDocTicketsDto.BonusType).ToLower()
                });
            }

            if (complexDocTicketsDto.PaymentType != null)
            {
                placeholders.Add(new Placeholder
                {
                    Key = "payment_type",
                    Value = Enum.GetName(typeof(PaymentTypeDto), complexDocTicketsDto.PaymentType).ToLower()
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.Cashier))
            {
                placeholders.Add(new Placeholder
                {
                    Key = "cashier",
                    Value = complexDocTicketsDto.Cashier
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.Comment))
            {
                placeholders.Add(new Placeholder
                {
                    Key = "comment",
                    Value = complexDocTicketsDto.Comment
                });
            }

            if (string.IsNullOrEmpty(complexDocTicketsDto.Email))
            {
                placeholders.Add(new Placeholder
                {
                    Key = "email",
                    Value = complexDocTicketsDto.Email
                });
            }

            return placeholders.ToArray();            
        }

        // Преобразует тип НДС из int? в VATType
        private VATType MapVATType(int? vatType)
        {
            switch (vatType)
            {
                case null:
                default:
                    return VATType.NoVAT;
                case 0:
                    return VATType.Percent0;
                case 10:
                    return VATType.Percent10;
                case 20:
                    return VATType.Percent20;
            }
        }
    }
}
