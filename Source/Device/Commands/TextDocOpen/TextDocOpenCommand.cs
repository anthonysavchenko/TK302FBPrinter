using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.TextDocOpen
{
    public class TextDocOpenCommand : DeviceCommand, ITextDocOpenCommand
    {
        public TextDocOpenCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig) {}

        public bool Execute()
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.OpenNotFiscalDocument(
                    _deviceConfig.OperatorPassword,
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
