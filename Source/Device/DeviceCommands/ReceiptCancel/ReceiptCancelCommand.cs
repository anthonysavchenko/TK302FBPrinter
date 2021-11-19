using Custom.Fiscal.RUSProtocolAPI;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptCancel
{
    public class ReceiptCancelCommand : DeviceCommand, IReceiptCancelCommand
    {
        private readonly PrinterOptions _printerOptions;
 
        public ReceiptCancelCommand(
            ProtocolAPI connection,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connection)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            var deviceResponse = _connection.CheckCancellation(_printerOptions.OperatorPassword);
            return !CheckRespose(deviceResponse);
        }
    }
}
