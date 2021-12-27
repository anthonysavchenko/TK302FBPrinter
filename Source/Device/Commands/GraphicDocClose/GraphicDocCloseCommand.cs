using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.GraphicDocClose
{
    public class GraphicDocCloseCommand : DeviceCommand, IGraphicDocCloseCommand
    {
        public GraphicDocCloseCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute()
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.GraphicTicketClose(
                    _deviceConfig.OperatorPassword,
                    Print: true,
                    saveOnFile: false,
                    // Если выключено автоматическое отрезание в настройках и этот параметр true,
                    // то печатает (с дополнительным отступом) и не отрезает
                    //
                    // Если выключено автоматическое отрезание в настройках и этот параметр false,
                    // то печатает и не отрезает
                    //
                    // Если включено автоматическое отрезание в настройках и этот параметр true,
                    // то печатает и отрезает
                    //
                    // Если включено автоматическое отрезание в настройках и этот параметр false,
                    // то печатает и не отрезает
                    cutPaper: false);

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
