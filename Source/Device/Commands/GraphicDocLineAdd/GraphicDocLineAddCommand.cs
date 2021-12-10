using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.GraphicDocLineAdd
{
    public class GraphicDocLineAddCommand : DeviceCommand, IGraphicDocLineAddCommand
    {
        public GraphicDocLineAddCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(int positionX1, int positionY1, int positionX2, int positionY2, int width)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.GraphicTicketLine(
                    _deviceConfig.OperatorPassword,
                    Print: true,
                    SaveOnFile: false,
                    LineX1: positionX1,
                    LineY1: positionY1,
                    LineX2: positionX2,
                    LineY2: positionY2,
                    LineWidth: width,
                    LineReverse: 0,
                    LinePattern: 0,
                    FillingPattern: 2); // 0 - прозрачный внутренний фон, 1 - заполненный

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
