using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.TextDocTextAdd;
using TK302FBPrinter.Device.Commands.TextDocClose;
using TK302FBPrinter.Device.Commands.TextDocOpen;
using TK302FBPrinter.Configuration;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Business.Models;

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

        public PrintSlipOperation(
            IOptions<SlipConfig> slipConfig,
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            ITextDocOpenCommand textDocOpenCommand,
            ITextDocTextAddCommand textDocTextAddCommand,
            ITextDocCloseCommand textDocCloseCommand)
        {
            _slipConfig = slipConfig.Value;
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _textDocOpenCommand = textDocOpenCommand;
            _textDocTextAddCommand = textDocTextAddCommand;
            _textDocCloseCommand = textDocCloseCommand;
        }

        public bool Execute(Slip slip)
        {
            var lines = slip.Text.Split(_slipConfig.LineSeparators, System.StringSplitOptions.None);

            if (slip.WithConnection && !_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_textDocOpenCommand.Execute())
            {
                AddErrorDescription(_textDocOpenCommand.ErrorDescription);
                Disconnect(slip.WithConnection);
                return false;
            }

            foreach (var line in lines)
            {
                if (!_textDocTextAddCommand.Execute(line))
                {
                    AddErrorDescription(_textDocTextAddCommand.ErrorDescription);
                    CloseDoc(cut: true);
                    Disconnect(slip.WithConnection);
                    return false;
                }
            }

            if (!CloseDoc(slip.Cut))
            {
                Disconnect(slip.WithConnection);
                return false;
            }

            Disconnect(slip.WithConnection);
            return true;
        }

        private void Disconnect(bool withConnection)
        {
            if (withConnection && !_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
            }
        }

        private bool CloseDoc(bool cut)
        {
            if (!_textDocCloseCommand.Execute(cut))
            {
                AddErrorDescription(_textDocCloseCommand.ErrorDescription);
                return false;
            }
            return true;
        }
    }
}
