using Custom.Fiscal.RUSProtocolAPI;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.Beep
{
    public class BeepCommand : DeviceCommand, IBeepCommand
    {
        private readonly PrinterOptions _printerOptions;

        public BeepCommand(
            ProtocolAPI connection,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connection)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            var deviceResponse = _connection.Beep(_printerOptions.OperatorPassword);
            return !CheckRespose(deviceResponse);
        }
    }
}
