using Custom.Fiscal.RUSProtocolAPI;

namespace TK302FBPrinter.Device.DeviceCommands.Disconnect
{
    public class DisconnectCommand : DeviceCommand, IDisconnectCommand
    {
        public DisconnectCommand(ProtocolAPI connection) : base(connection)
        {
        }

        public bool Execute()
        {
            if (_connection == null)
            {
                return true;
            }
            var deviceResponse = _connection.CloseConnection();
            return CheckRespose(deviceResponse);
        }
    }
}
