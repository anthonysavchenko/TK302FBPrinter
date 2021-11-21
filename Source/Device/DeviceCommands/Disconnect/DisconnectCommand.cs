using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.Disconnect
{
    public class DisconnectCommand : DeviceCommand, IDisconnectCommand
    {
        public DisconnectCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig) {}

        public bool Execute()
        {
            if (_deviceConnector.Connection == null)
            {
                return true;
            }

            try
            {
                var deviceResponse = _deviceConnector.Connection.CloseConnection();
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
