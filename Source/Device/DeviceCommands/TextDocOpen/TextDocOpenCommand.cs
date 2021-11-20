using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.TextDocOpen
{
    public class TextDocOpenCommand : DeviceCommand, ITextDocOpenCommand
    {
        private readonly PrinterOptions _printerOptions;

        public TextDocOpenCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            try
            {
                var deviceResponse = _connector.Connection.OpenNotFiscalDocument(
                    _printerOptions.OperatorPassword,
                    printOperator: false,
                    printSerialNum: false,
                    printHeader: false,
                    printDateTime: false);

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
