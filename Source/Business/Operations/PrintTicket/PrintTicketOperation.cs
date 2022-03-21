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
using TK302FBPrinter.Device.Commands.Cut;
using System.Globalization;

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
        private readonly ICutCommand _cutCommand;

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
            ICutCommand cutCommand)
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
            _cutCommand = cutCommand;
        }

        public bool Execute(Ticket ticket, bool cut = false)
        {
            var template = System.Array.Find(_ticketConfig.Templates, t => t.TemplateName == ticket.TemplateName);

            if (ticket.WithConnection && !_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_graphicDocOpenCommand.Execute(template.SizeX, template.SizeY))
            {
                AddErrorDescription(_graphicDocOpenCommand.ErrorDescription);
                Disconnect(ticket.WithConnection);
                return false;
            }

            if (!PrintLines(template.Lines))
            {
                Disconnect(ticket.WithConnection);
                return false;
            }

            if (!PrintTextLines(template.TextLines, ticket.Placeholders))
            {
                Disconnect(ticket.WithConnection);
                return false;
            }

            if (!PrintSeats(ticket.Seats, template.SeatTextLines))
            {
                Disconnect(ticket.WithConnection);
                return false;
            }

            if (!PrintQrCodes(template.QrCodes, ticket.Placeholders))
            {
                Disconnect(ticket.WithConnection);
                return false;
            }

            if (!PrintBitmaps(template.Bitmaps))
            {
                Disconnect(ticket.WithConnection);
                return false;
            }

            if (!_graphicDocCloseCommand.Execute())
            {
                AddErrorDescription(_graphicDocCloseCommand.ErrorDescription);
                Disconnect(ticket.WithConnection);
                return false;
            }

            if ((cut || ticket.Cut) && !_cutCommand.Execute())
            {
                AddErrorDescription(_cutCommand.ErrorDescription);
                return false;
            }

            Disconnect(ticket.WithConnection);
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
                    text = text.Replace(
                        placeholder.Key,
                        placeholder.Value,
                        ignoreCase: true,
                        CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrWhiteSpace(text) && !_graphicDocTextAddCommand.Execute(
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

                if (text != textLine.Text && !string.IsNullOrWhiteSpace(text) && !_graphicDocTextAddCommand.Execute(
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

        private bool PrintQrCodes(QrCode[] qrCodes, Placeholder[] placeholders)
        {
            foreach (var qrCode in qrCodes)
            {
                var qrCodeText = qrCode.Text;

                foreach (var placeholder in placeholders)
                {
                    qrCodeText = qrCodeText.Replace(
                        placeholder.Key,
                        placeholder.Value,
                        ignoreCase: true,
                        CultureInfo.InvariantCulture);
                }

                if (!_graphicDocQrCodeAddCommand.Execute(
                    qrCodeText,
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
                    return false;
                }
            }

            return true;
        }

        private void Disconnect(bool withConnection)
        {
            if (withConnection && !_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
            }
        }
    }
}
