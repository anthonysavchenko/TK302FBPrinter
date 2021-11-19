using Custom.Fiscal.RUSProtocolAPI;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.CancelLastItem
{
    public class CancelLastItemCommand : DeviceCommand, ICancelLastItemCommand
    {
        private readonly PrinterOptions _printerOptions;

        public CancelLastItemCommand(
            ProtocolAPI connection,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connection)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            var deviceResponse = _connection.VoidLastItem(_printerOptions.OperatorPassword);
            return !CheckRespose(deviceResponse);
        }
    }
}
