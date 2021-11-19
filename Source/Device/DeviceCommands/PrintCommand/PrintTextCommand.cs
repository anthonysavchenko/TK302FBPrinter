using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.PrintCommand
{
    public class PrintTextCommand : DeviceCommand, IPrintTextCommand
    {
        private readonly PrinterOptions _printerOptions;

        public PrintTextCommand(
            ProtocolAPI connection,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connection)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute(string text)
        {
            var deviceResponse = _connection.PrintText(
                _printerOptions.OperatorPassword,
                fontSize: FontSizeEnum.DefaultSize,
                doubleWidth: false,
                doubleHeigth: false,
                halfWidth: false,
                halfHeigth: false,
                border: false,
                bold: false,
                italic: false,
                automaticNewLine: true,
                text: text);

            return !CheckRespose(deviceResponse);
        }
    }
}
