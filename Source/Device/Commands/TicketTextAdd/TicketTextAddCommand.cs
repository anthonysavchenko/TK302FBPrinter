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

        public bool Execute(
            string text,
            int rotation = 2,
            int positionX = 1,
            int positionY = 1,
            int fontSize = 3,
            int scaleX = 1,
            int scaleY = 1,
            int fontStyle = 11)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.GraphicTicketText(
                    _deviceConfig.OperatorPassword,
                    Print: true,
                    SaveOnFile: false,
                    TextRotation: rotation, // 0 - 0 градусов, 1 - 90 градусов, 2 - 180 градусов, 3 - 270 градусов
                    TextX: positionX,
                    TextY: positionY,
                    TextScaleX: scaleX, // 1 - 3?
                    TextScaleY: scaleY, // 1 - 3?
                    FontSize: fontSize, // 1 - 5
                    FontStyle: fontStyle, // 10 - bold, 11 - no bold, 12 - italic, 13 - no italic
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
