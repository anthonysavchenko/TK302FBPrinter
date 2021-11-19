using Custom.Fiscal.RUSProtocolAPI;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.ShiftClose
{
    public class ShiftCloseCommand : DeviceCommand, IShiftCloseCommand
    {
        private readonly PrinterOptions _printerOptions;

        public ShiftCloseCommand(
            ProtocolAPI connection,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connection)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            bool print = true; // Печатаь фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)
            
            var deviceResponse = _connection.ZReport(_printerOptions.OperatorPassword, print, saveOnFile);
            return !CheckRespose(deviceResponse);
        }
    }
}
