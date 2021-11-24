using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.TicketClose;
using TK302FBPrinter.Device.Commands.TicketOpen;
using TK302FBPrinter.Device.Commands.TicketTextAdd;

namespace TK302FBPrinter.Business.Operations.PrintTicket
{
    public class PrintTicketOperation : Operation, IPrintTicketOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly ITicketOpenCommand _ticketOpenCommand;
        private readonly ITicketCloseCommand _ticketCloseCommand;
        private readonly ITicketTextAddCommand _ticketTextAddCommand;

        public PrintTicketOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            ITicketOpenCommand ticketOpenCommand,
            ITicketCloseCommand ticketCloseCommand,
            ITicketTextAddCommand ticketTextAddCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _ticketOpenCommand = ticketOpenCommand;
            _ticketCloseCommand = ticketCloseCommand;
            _ticketTextAddCommand = ticketTextAddCommand;
        }

        public bool Execute(string text)
        {
            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_ticketOpenCommand.Execute(576, 1100))
            {
                AddErrorDescription(_ticketOpenCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            if (!_ticketTextAddCommand.Execute(text, 300, 150))
            {
                AddErrorDescription(_ticketTextAddCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            if (!_ticketCloseCommand.Execute())
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
