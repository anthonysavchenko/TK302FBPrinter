using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.GraphicDocOpen
{
    public class GraphicDocOpenCommand : DeviceCommand, IGraphicDocOpenCommand
    {
        public GraphicDocOpenCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(int sizeX, int sizeY)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.GraphicTicketOpen(
                    _deviceConfig.OperatorPassword,
                    ticketName: "ticket",
                    ticketRotation: true,
                    ticketXSize: sizeX,
                    ticketYSize: sizeY);

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
