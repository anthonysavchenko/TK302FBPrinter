using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.TextDocClose
{
    public class TextDocCloseCommand : DeviceCommand, ITextDocCloseCommand
    {
        public TextDocCloseCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute()
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.CloseNotFiscallDocument(
                    _deviceConfig.OperatorPassword,
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
