using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.TextDocClose
{
    public class TextDocCloseCommand : DeviceCommand, ITextDocCloseCommand
    {
        private readonly PrinterOptions _printerOptions;

        public TextDocCloseCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            try
            {
                var deviceResponse = _connector.Connection.CloseNotFiscallDocument(
                    _printerOptions.OperatorPassword,
                    printSerialNum: false,
                    paperCut: true);

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
