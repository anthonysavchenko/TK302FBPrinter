using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.TextDocTextAdd;
using TK302FBPrinter.Device.Commands.TextDocClose;
using TK302FBPrinter.Device.Commands.TextDocOpen;

namespace TK302FBPrinter.Business.Operations.PrintSlip
{
    public class PrintSlipOperation : Operation, IPrintSlipOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly ITextDocOpenCommand _textDocOpenCommand;
        private readonly ITextDocTextAddCommand _textDocTextAddCommand;
        private readonly ITextDocCloseCommand _textDocCloseCommand;

        public PrintSlipOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            ITextDocOpenCommand textDocOpenCommand,
            ITextDocTextAddCommand textDocTextAddCommand,
            ITextDocCloseCommand textDocCloseCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _textDocOpenCommand = textDocOpenCommand;
            _textDocTextAddCommand = textDocTextAddCommand;
            _textDocCloseCommand = textDocCloseCommand;
        }
        
        public bool Execute(string text)
        {
            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_textDocOpenCommand.Execute())
            {
                AddErrorDescription(_textDocOpenCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            if (!_textDocTextAddCommand.Execute(text))
            {
                AddErrorDescription(_textDocTextAddCommand.ErrorDescription);
                CloseDoc();
                Disconnect();
                return false;
            }

            if (!CloseDoc())
            {
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

        private bool CloseDoc()
        {
            if (!_textDocCloseCommand.Execute())
            {
                AddErrorDescription(_textDocCloseCommand.ErrorDescription);
                return false;
            }
            return true;
        }
    }
}
