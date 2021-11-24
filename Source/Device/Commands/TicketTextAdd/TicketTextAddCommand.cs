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
                    TextRotation: 2,
                    TextX: xPosition,
                    TextY: yPosition,
                    TextScaleX: 2,
                    TextScaleY: 2,
                    FontSize: 3,
                    FontStyle: 2,
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
