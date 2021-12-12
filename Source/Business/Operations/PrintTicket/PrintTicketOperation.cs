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

            if (!PrintTicket(template, ticket, cut: ticket.Receipt == null))
            {
                return false;
            }

            if (ticket.Receipt != null && !_printReceiptOperation.Execute(ticket.Receipt))
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
            var slipLines = !string.IsNullOrEmpty(ticket.SlipText)
                ? ticket.SlipText.Split(_ticketConfig.SlipLineSeparators, System.StringSplitOptions.None)
                : null;

            var slipHeight = slipLines != null ? (slipLines.Length + 1) * 35 : 0;

            if (!_graphicDocOpenCommand.Execute(template.SizeX, template.SizeY + slipHeight))
            {
                AddErrorDescription(_graphicDocOpenCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            if (slipLines != null && !PrintSlipLines(slipLines))
            {
                Disconnect();
                return false;
            }

            if (!PrintLines(template.Lines, slipHeight))
            {
                Disconnect();
                return false;
            }

            if (!PrintTextLines(template.TextLines, ticket.Placeholders, slipHeight))
            {
                Disconnect();
                return false;
            }

            if (!PrintSeats(ticket.Seats, template.SeatTextLines, slipHeight))
            {
                Disconnect();
                return false;
            }

            if (!PrintQrCodes(template.QrCodes, slipHeight))
            {
                Disconnect();
                return false;
            }

            if (!PrintBitmaps(template.Bitmaps, slipHeight))
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

        private bool PrintSlipLines(string[] slipLines)
        {
            for (var index = 0; index < slipLines.Length; index++)
            {
                if (!string.IsNullOrEmpty(slipLines[index])
                    && !_graphicDocTextAddCommand.Execute(
                        slipLines[index],
                        rotation: 1,
                        positionX: 0,
                        positionY: index * 35,
                        fontSize: 3,
                        scaleX: 1,
                        scaleY: 2,
                        fontStyle: 11))
                {
                    AddErrorDescription(_graphicDocTextAddCommand.ErrorDescription);
                    return false;
                }
            }

            return true;
        }

        private bool PrintLines(Line[] lines, int offsetY)
        {
            foreach (var line in lines)
            {
                if (!_graphicDocLineAddCommand.Execute(
                    line.PositionX1,
                    line.PositionY1 + offsetY,
                    line.PositionX2,
                    line.PositionY2 + offsetY,
                    line.Width))
                {
                    AddErrorDescription(_graphicDocLineAddCommand.ErrorDescription);
                    return false;
                }
            }

            return true;
        }

        private bool PrintTextLines(TextLine[] textLines, Placeholder[] placeholders, int offsetY)
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
                    textLine.PositionY + offsetY,
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

        private bool PrintSeats(Seat[] seats, TextLine[] seatTextLines, int offsetY)
        {
            var seatNames = seats
                .Select(x => _ticketConfig.SeatsName
                    .Replace(_ticketConfig.SeatsRowPlaceholder, x.Row.ToString())
                    .Replace(_ticketConfig.SeatsPlacePlaceholder, x.Place.ToString()))
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

                for (int index = 0; index < 20; index++)
                {
                    text = text.Replace(_ticketConfig.SeatsPlaceholder.Replace("1", $"{index + 1}"), string.Empty);
                }

                if (text != textLine.Text && !string.IsNullOrWhiteSpace(text))
                {
                    if (!_graphicDocTextAddCommand.Execute(
                        text,
                        textLine.Rotation,
                        textLine.PositionX,
                        textLine.PositionY + offsetY,
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

        private bool PrintQrCodes(QrCode[] qrCodes, int offsetY)
        {
            foreach (var qrCode in qrCodes)
            {
                if (!_graphicDocQrCodeAddCommand.Execute(
                    qrCode.Text,
                    qrCode.Rotation,
                    qrCode.PositionX,
                    qrCode.PositionY + offsetY,
                    qrCode.Scale
                ))
                {
                    AddErrorDescription(_graphicDocQrCodeAddCommand.ErrorDescription);
                    return false;
                }
            }

            return true;
        }

        private bool PrintBitmaps(Bitmap[] bitmaps, int offsetY)
        {
            foreach (var bitmap in bitmaps)
            {
                if (!_graphicDocBitmapAddCommand.Execute(
                    bitmap.BitmapId,
                    bitmap.Rotation,
                    bitmap.PositionX,
                    bitmap.PositionY + offsetY,
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
