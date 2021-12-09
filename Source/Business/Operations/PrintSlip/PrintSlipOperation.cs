using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.TextDocTextPrint;
using TK302FBPrinter.Device.Commands.TextDocClose;
using TK302FBPrinter.Device.Commands.TextDocOpen;

namespace TK302FBPrinter.Business.Operations.PrintSlip
{
    public class PrintSlipOperation : Operation, IPrintSlipOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly ITextDocOpenCommand _textDocOpenCommand;
        private readonly ITextDocTextPrintCommand _textDocTextPrintCommand;
        private readonly ITextDocCloseCommand _textDocCloseCommand;

        public PrintSlipOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            ITextDocOpenCommand textDocOpenCommand,
            ITextDocTextPrintCommand textDocTextPrintCommand,
            ITextDocCloseCommand textDocCloseCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _textDocOpenCommand = textDocOpenCommand;
            _textDocTextPrintCommand = textDocTextPrintCommand;
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

            if (!_textDocTextPrintCommand.Execute(text))
            {
                AddErrorDescription(_textDocTextPrintCommand.ErrorDescription);
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
