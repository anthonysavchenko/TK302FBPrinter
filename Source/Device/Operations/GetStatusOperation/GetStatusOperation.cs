using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;
using TK302FBPrinter.Device.DeviceCommands.PrinterStatusGet;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Operations.GetStatusOperation
{
    public class GetStatusOperation : Operation, IGetStatusOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IPrinterStatusGetCommand _printerStatusGetCommand;

        public GetStatusOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IPrinterStatusGetCommand printerStatusGetCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _printerStatusGetCommand = printerStatusGetCommand;
        }
        public bool Execute(out PrinterStatusDto status)
        {
            status = null;

            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_printerStatusGetCommand.Execute(out status))
            {
                AddErrorDescription(_printerStatusGetCommand.ErrorDescription);
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
