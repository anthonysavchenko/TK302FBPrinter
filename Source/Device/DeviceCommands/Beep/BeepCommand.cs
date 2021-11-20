using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.Beep
{
    public class BeepCommand : DeviceCommand, IBeepCommand
    {
        private readonly PrinterOptions _printerOptions;

        public BeepCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            try
            {
                var deviceResponse = _connector.Connection.Beep(_printerOptions.OperatorPassword);
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
