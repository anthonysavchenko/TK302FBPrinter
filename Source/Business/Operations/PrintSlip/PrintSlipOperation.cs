using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.TextDocTextAdd;
using TK302FBPrinter.Device.Commands.TextDocClose;
using TK302FBPrinter.Device.Commands.TextDocOpen;
using TK302FBPrinter.Configuration;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Business.Models;
using TK302FBPrinter.Device.Commands.Cut;

namespace TK302FBPrinter.Business.Operations.PrintSlip
{
    public class PrintSlipOperation : Operation, IPrintSlipOperation
    {
        private readonly SlipConfig _slipConfig;
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly ITextDocOpenCommand _textDocOpenCommand;
        private readonly ITextDocTextAddCommand _textDocTextAddCommand;
        private readonly ITextDocCloseCommand _textDocCloseCommand;
        private readonly ICutCommand _cutCommand;

        public PrintSlipOperation(
            IOptions<SlipConfig> slipConfig,
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            ITextDocOpenCommand textDocOpenCommand,
            ITextDocTextAddCommand textDocTextAddCommand,
            ITextDocCloseCommand textDocCloseCommand,
            ICutCommand cutCommand)
        {
            _slipConfig = slipConfig.Value;
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _textDocOpenCommand = textDocOpenCommand;
            _textDocTextAddCommand = textDocTextAddCommand;
            _textDocCloseCommand = textDocCloseCommand;
            _cutCommand = cutCommand;
        }

        public bool Execute(Slip slip)
        {
            var lines = slip.Text.Split(_slipConfig.LineSeparators, System.StringSplitOptions.None);

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

            foreach (var line in lines)
            {
                if (!_textDocTextAddCommand.Execute(line))
                {
                    AddErrorDescription(_textDocTextAddCommand.ErrorDescription);
                    CloseDoc(slip.Cut);
                    Disconnect();
                    return false;
                }
            }

            if (!CloseDoc(slip.Cut))
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

        private bool CloseDoc(bool cut)
        {
            if (!_textDocCloseCommand.Execute())
            {
                AddErrorDescription(_textDocCloseCommand.ErrorDescription);
                return false;
            }

            if (cut && !_cutCommand.Execute())
            {
                AddErrorDescription(_cutCommand.ErrorDescription);
                return false;
            }

            return true;
        }
    }
}
