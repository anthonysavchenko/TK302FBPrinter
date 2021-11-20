using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptCancel
{
    public class ReceiptCancelCommand : DeviceCommand, IReceiptCancelCommand
    {
        private readonly PrinterOptions _printerOptions;
 
        public ReceiptCancelCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            try
            {
                var deviceResponse = _connector.Connection.CheckCancellation(_printerOptions.OperatorPassword);
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
