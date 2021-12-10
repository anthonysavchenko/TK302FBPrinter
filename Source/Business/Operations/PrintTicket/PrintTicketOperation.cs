using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.TicketClose;
using TK302FBPrinter.Device.Commands.GraphicDocOpen;
using TK302FBPrinter.Device.Commands.GraphicDocTextAdd;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Business.Operations.PrintTicket
{
    public class PrintTicketOperation : Operation, IPrintTicketOperation
    {
        private readonly TicketConfig _ticketConfig;
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IGraphicDocOpenCommand _graphicDocOpenCommand;
        private readonly ITicketCloseCommand _ticketCloseCommand;
        private readonly IGraphicDocTextAddCommand _graphicDocTextAddCommand;

        public PrintTicketOperation(
            IOptions<TicketConfig> ticketConfig,
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IGraphicDocOpenCommand graphicDocOpenCommand,
            ITicketCloseCommand ticketCloseCommand,
            IGraphicDocTextAddCommand graphicDocTextAddCommand)
        {
            _ticketConfig = ticketConfig.Value;
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _graphicDocOpenCommand = graphicDocOpenCommand;
            _ticketCloseCommand = ticketCloseCommand;
            _graphicDocTextAddCommand = graphicDocTextAddCommand;
        }

        public bool Execute(TicketDto ticket)
        {
            var template = System.Array.Find(_ticketConfig.Templates, t => t.TemplateName == ticket.TemplateName);

            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_graphicDocOpenCommand.Execute(576, 1100))
            {
                AddErrorDescription(_graphicDocOpenCommand.ErrorDescription);
                Disconnect();
                return false;
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

            if (!_ticketCloseCommand.Execute(cut: true))
            {
                AddErrorDescription(_ticketCloseCommand.ErrorDescription);
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
