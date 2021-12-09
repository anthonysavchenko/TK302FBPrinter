using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.TicketTextAdd
{
    public class TicketTextAddCommand : DeviceCommand, ITicketTextAddCommand
    {
        public TicketTextAddCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(string text, int xPosition, int yPosition)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.GraphicTicketText(
                    _deviceConfig.OperatorPassword,
                    Print: true,
                    SaveOnFile: false,
                    TextRotation: 2, // 0 - 0 градусов, 1 - 90 градусов, 2 - 180 градусов, 3 - 270 градусов
                    TextX: xPosition,
                    TextY: yPosition,
                    TextScaleX: 2, // 1 - 3?
                    TextScaleY: 2, // 1 - 3?
                    FontSize: 3, // 1 - 5
                    FontStyle: 10, // 10 - bold, 11 - no bold, 12 - italic, 13 - no italic
                    Text: text);

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
