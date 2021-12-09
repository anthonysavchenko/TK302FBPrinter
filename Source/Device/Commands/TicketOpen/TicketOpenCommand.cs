using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.TicketOpen
{
    public class TicketOpenCommand : DeviceCommand, ITicketOpenCommand
    {
        public TicketOpenCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(int xSize, int ySize)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.GraphicTicketOpen(
                    _deviceConfig.OperatorPassword,
                    ticketName: "ticket",
                    ticketRotation: true,
                    ticketXSize: xSize,
                    ticketYSize: ySize);

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
