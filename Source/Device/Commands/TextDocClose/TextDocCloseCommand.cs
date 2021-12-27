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
                    // Если выключено автоматическое отрезание в настройках и этот параметр true,
                    // то печатает, но не отрезает
                    //
                    // Если выключено автоматическое отрезание в настройках и этот параметр false,
                    // то не печатает и не отрезает
                    //
                    // Если включено автоматическое отрезание в настройках и этот параметр true,
                    // то печатает, и отрезает
                    //
                    // Если включено автоматическое отрезание в настройках и этот параметр false,
                    // то не печатает и не отрезает
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
