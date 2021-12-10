using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.GraphicDocClose;
using TK302FBPrinter.Device.Commands.GraphicDocOpen;
using TK302FBPrinter.Device.Commands.GraphicDocTextAdd;
using TK302FBPrinter.Device.Commands.GraphicDocLineAdd;
using TK302FBPrinter.Dto;
using TK302FBPrinter.Device.Commands.GraphicDocQrCodeAdd;
using TK302FBPrinter.Device.Commands.GraphicDocBitmapAdd;

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

        public PrintTicketOperation(
            IOptions<TicketConfig> ticketConfig,
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IGraphicDocOpenCommand graphicDocOpenCommand,
            IGraphicDocCloseCommand graphicDocCloseCommand,
            IGraphicDocTextAddCommand graphicDocTextAddCommand,
            IGraphicDocLineAddCommand graphicDocLineAddCommand,
            IGraphicDocQrCodeAddCommand graphicDocQrCodeAddCommand,
            IGraphicDocBitmapAddCommand graphicDocBitmapAddCommand)
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
        }

        public bool Execute(TicketDto ticket)
        {
            var template = System.Array.Find(_ticketConfig.Templates, t => t.TemplateName == ticket.TemplateName);

            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_graphicDocOpenCommand.Execute(template.SizeX, template.SizeY))
            {
                AddErrorDescription(_graphicDocOpenCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            foreach (var line in template.Lines)
            {
                if (!_graphicDocLineAddCommand.Execute(
                    line.PositionX1,
                    line.PositionY1,
                    line.PositionX2,
                    line.PositionY2,
                    line.Width))
                {
                    AddErrorDescription(_graphicDocLineAddCommand.ErrorDescription);
                    Disconnect();
                    return false;
                }
            }

            foreach (var textLine in template.TextLines)
            {
                var text = textLine.Text;

                foreach (var placeholder in ticket.Placeholders)
                {
                    text = text.Replace(placeholder.Key, placeholder.Replacement);
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
                    Disconnect();
                    return false;
                }
            }

            foreach (var qrCode in template.QrCodes)
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
                    Disconnect();
                    return false;
                }
            }

            foreach (var bitmap in template.Bitmaps)
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

            if (!_graphicDocCloseCommand.Execute(cut: true))
            {
                AddErrorDescription(_graphicDocCloseCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            Disconnect();
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
