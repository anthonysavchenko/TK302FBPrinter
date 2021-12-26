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
                    // По какой-то причине отрезание бумаги не происходит при выставлении этого параметра в true.
                    // Слип-чек отрезается только, если в настройках принтера указано автоматическое отрезание.
                    // А при выставлении его в false слип-чек не печатается вовсе.
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
