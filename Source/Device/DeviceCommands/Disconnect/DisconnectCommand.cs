using System;

namespace TK302FBPrinter.Device.DeviceCommands.Disconnect
{
    public class DisconnectCommand : DeviceCommand, IDisconnectCommand
    {
        public DisconnectCommand(Connector connector) : base(connector) {}

        public bool Execute()
        {
            if (_connector.Connection == null)
            {
                return true;
            }

            try
            {
                var deviceResponse = _connector.Connection.CloseConnection();
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
