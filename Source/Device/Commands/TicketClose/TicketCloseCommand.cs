using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.TicketClose
{
    public class TicketCloseCommand : DeviceCommand, ITicketCloseCommand
    {
        public TicketCloseCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig) {}

        public bool Execute()
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.GraphicTicketClose(
                    _deviceConfig.OperatorPassword,
                    Print: true,
                    saveOnFile: false,
                    cutPaper: true);

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
