using System;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.PrintTextCommand
{
    public class PrintTextCommand : DeviceCommand, IPrintTextCommand
    {
        private readonly PrinterOptions _printerOptions;

        public PrintTextCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute(string text)
        {
            try
            {
                var deviceResponse = _connector.Connection.PrintText(
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

                return CheckRespose(deviceResponse);
            }
            catch (Exception exception)
            {
                AddException(exception);
                return false;
            }
        }
    }
}
