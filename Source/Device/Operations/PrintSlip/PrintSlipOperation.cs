using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;
using TK302FBPrinter.Device.DeviceCommands.PrintCommand;

namespace TK302FBPrinter.Device.Operations.PrintSlip
{
    public class PrintSlipOperation : Operation, IPrintSlipOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IPrintTextCommand _printTextCommand;

        public PrintSlipOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IPrintTextCommand printTextCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _printTextCommand = printTextCommand;
        }
        
        public bool Execute(string text)
        {
            bool result = true;

            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_printTextCommand.Execute(text))
            {
                AddErrorDescription(_printTextCommand.ErrorDescription);
                result = false;
            }

            if (!_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
                return false;
            }

            return result;
        }
    }
}
