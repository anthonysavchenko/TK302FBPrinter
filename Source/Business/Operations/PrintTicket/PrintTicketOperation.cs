using System.Linq;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.GraphicDocClose;
using TK302FBPrinter.Device.Commands.GraphicDocOpen;
using TK302FBPrinter.Device.Commands.GraphicDocTextAdd;
using TK302FBPrinter.Device.Commands.GraphicDocLineAdd;
using TK302FBPrinter.Device.Commands.GraphicDocQrCodeAdd;
using TK302FBPrinter.Device.Commands.GraphicDocBitmapAdd;
using TK302FBPrinter.Business.Models;
using TK302FBPrinter.Business.Operations.PrintSlip;
using TK302FBPrinter.Business.Operations.PrintReceipt;

namespace TK302FBPrinter.Business.Operations.PrintTicket
{
    public class PrintTicketOperation : Operation, IPrintTicketOperation
    {
        private readonly TicketConfig _ticketConfig;
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IGraphicDocOpenCommand _graphicDocOpenCommand;
        private readonly IGraphicDocCloseCommand _graphicDocCloseCommand;
        private readonly IGraphicDocTextAddCommand _graphicDocTextAddCommand;
        private readonly IGraphicDocLineAddCommand _graphicDocLineAddCommand;
        private readonly IGraphicDocQrCodeAddCommand _graphicDocQrCodeAddCommand;
        private readonly IGraphicDocBitmapAddCommand _graphicDocBitmapAddCommand;
        private readonly IPrintSlipOperation _printSlipOperation;
        private readonly IPrintReceiptOperation _printReceiptOperation;

        public PrintTicketOperation(
            IOptions<TicketConfig> ticketConfig,
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IGraphicDocOpenCommand graphicDocOpenCommand,
            IGraphicDocCloseCommand graphicDocCloseCommand,
            IGraphicDocTextAddCommand graphicDocTextAddCommand,
            IGraphicDocLineAddCommand graphicDocLineAddCommand,
            IGraphicDocQrCodeAddCommand graphicDocQrCodeAddCommand,
            IGraphicDocBitmapAddCommand graphicDocBitmapAddCommand,
            IPrintSlipOperation printSlipOperation,
            IPrintReceiptOperation printReceiptOperation)
        {
            _ticketConfig = ticketConfig.Value;
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _graphicDocOpenCommand = graphicDocOpenCommand;
            _graphicDocCloseCommand = graphicDocCloseCommand;
            _graphicDocTextAddCommand = graphicDocTextAddCommand;
            _graphicDocLineAddCommand = graphicDocLineAddCommand;
            _graphicDocQrCodeAddCommand = graphicDocQrCodeAddCommand;
            _graphicDocBitmapAddCommand = graphicDocBitmapAddCommand;
            _printSlipOperation = printSlipOperation;
            _printReceiptOperation = printReceiptOperation;
        }

        public bool Execute(Ticket ticket)
        {
            var template = System.Array.Find(_ticketConfig.Templates, t => t.TemplateName == ticket.TemplateName);

            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_printSlipOperation.Execute(ticket.Slip))
            {
                AddErrorDescription(_printSlipOperation.ErrorDescriptions);
                Disconnect();
                return false;
            }

            if (!PrintTicket(template, ticket, cut: ticket.Receipt == null))
            {
                return false;
            }

            if (!_printReceiptOperation.Execute(ticket.Receipt))
            {
                AddErrorDescription(_printReceiptOperation.ErrorDescriptions);
                Disconnect();
                return false;
            }

            Disconnect();
            return true;
        }

        private bool PrintTicket(Template template, Ticket ticket, bool cut)
        {
            if (!_graphicDocOpenCommand.Execute(template.SizeX, template.SizeY))
            {
                AddErrorDescription(_graphicDocOpenCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            if (!PrintLines(template.Lines))
            {
                Disconnect();
                return false;
            }

            if (!PrintTextLines(template.TextLines, ticket.Placeholders))
            {
                Disconnect();
                return false;
            }

            if (!PrintSeats(ticket.Seats, template.SeatTextLines))
            {
                Disconnect();
                return false;
            }

            if (!PrintQrCodes(template.QrCodes))
            {
                Disconnect();
                return false;
            }

            if (!PrintBitmaps(template.Bitmaps))
            {
                Disconnect();
                return false;
            }

            if (!_graphicDocCloseCommand.Execute(cut))
            {
                AddErrorDescription(_graphicDocCloseCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            return true;
        }

        private bool PrintLines(Line[] lines)
        {
            foreach (var line in lines)
            {
                if (!_graphicDocLineAddCommand.Execute(
                    line.PositionX1,
                    line.PositionY1,
                    line.PositionX2,
                    line.PositionY2,
                    line.Width))
                {
                    AddErrorDescription(_graphicDocLineAddCommand.ErrorDescription);
                    return false;
                }
            }

            return true;
        }

        private bool PrintTextLines(TextLine[] textLines, Placeholder[] placeholders)
        {
            foreach (var textLine in textLines)
            {
                var text = textLine.Text;

                foreach (var placeholder in placeholders)
                {
                    text = text.Replace(placeholder.Key, placeholder.Value);
                }

                if (!_graphicDocTextAddCommand.Execute(
                    text,
                    textLine.Rotation,
                    textLine.PositionX,
                    textLine.PositionY,
                    textLine.FontSize,
                    textLine.ScaleX,
                    textLine.ScaleY,
                    textLine.FontStyle))
                {
                    AddErrorDescription(_graphicDocTextAddCommand.ErrorDescription);
                    return false;
                }
            }

            return true;
        }

        private bool PrintSeats(Seat[] seats, TextLine[] seatTextLines)
        {
            var seatNames = seats
                .Select(x => _ticketConfig.SeatsName
                    .Replace(_ticketConfig.SeatsRowPlaceholder, x.Row.ToString())
                    .Replace(_ticketConfig.SeatsPlaceholder, x.Place.ToString()))
                .ToArray();

            foreach (var textLine in seatTextLines)
            {
                var text = textLine.Text;

                for (var index = 0; index < seatNames.Length; index++)
                {
                    var seatName = seatNames[index];

                    if (index == 0 && !string.IsNullOrEmpty(seatName))
                    {
                        seatName = seatName.Substring(0, 1).ToUpper() + seatName.Substring(1);
                    }

                    if (index + 1 < seatNames.Length)
                    {
                        seatName += _ticketConfig.SeatsSeparator;
                    }
                    
                    text = text.Replace(_ticketConfig.SeatsPlaceholder.Replace("1", $"{index + 1}"), seatName);
                }

                if (text != textLine.Text)
                {
                    if (!_graphicDocTextAddCommand.Execute(
                        text,
                        textLine.Rotation,
                        textLine.PositionX,
                        textLine.PositionY,
                        textLine.FontSize,
                        textLine.ScaleX,
                        textLine.ScaleY,
                        textLine.FontStyle))
                    {
                        AddErrorDescription(_graphicDocTextAddCommand.ErrorDescription);
                        return false;
                    }
                }
            }

            return true;
        }

        private bool PrintQrCodes(QrCode[] qrCodes)
        {
            foreach (var qrCode in qrCodes)
            {
                if (!_graphicDocQrCodeAddCommand.Execute(
                    qrCode.Text,
                    qrCode.Rotation,
                    qrCode.PositionX,
                    qrCode.PositionY,
                    qrCode.Scale
                ))
                {
                    AddErrorDescription(_graphicDocQrCodeAddCommand.ErrorDescription);
                    return false;
                }
            }

            return true;
        }

        private bool PrintBitmaps(Bitmap[] bitmaps)
        {
            foreach (var bitmap in bitmaps)
            {
                if (!_graphicDocBitmapAddCommand.Execute(
                    bitmap.BitmapId,
                    bitmap.Rotation,
                    bitmap.PositionX,
                    bitmap.PositionY,
                    bitmap.ScaleX,
                    bitmap.ScaleY))
                {
                    AddErrorDescription(_graphicDocBitmapAddCommand.ErrorDescription);
                    Disconnect();
                    return false;
                }
            }

            return true;
        }

        private void Disconnect()
        {
            if (!_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
            }
        }
    }
}
