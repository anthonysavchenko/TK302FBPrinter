using System;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.PrintTextCommand
{
    public class PrintTextCommand : DeviceCommand, IPrintTextCommand
    {
        public PrintTextCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig) {}

        public bool Execute(string text)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.PrintText(
                    _deviceConfig.OperatorPassword,
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
