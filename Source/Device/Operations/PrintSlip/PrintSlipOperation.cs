using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;
using TK302FBPrinter.Device.DeviceCommands.PrintCommand;
using TK302FBPrinter.Device.DeviceCommands.TextDocClose;
using TK302FBPrinter.Device.DeviceCommands.TextDocOpen;

namespace TK302FBPrinter.Device.Operations.PrintSlip
{
    public class PrintSlipOperation : Operation, IPrintSlipOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly ITextDocOpenCommand _textDocOpenCommand;
        private readonly IPrintTextCommand _printTextCommand;
        private readonly ITextDocCloseCommand _textDocCloseCommand;

        public PrintSlipOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            ITextDocOpenCommand textDocOpenCommand,
            IPrintTextCommand printTextCommand,
            ITextDocCloseCommand textDocCloseCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _textDocOpenCommand = textDocOpenCommand;
            _printTextCommand = printTextCommand;
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

            if (!_printTextCommand.Execute(text))
            {
                AddErrorDescription(_printTextCommand.ErrorDescription);
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
