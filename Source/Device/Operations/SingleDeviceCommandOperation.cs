using TK302FBPrinter.Device.DeviceCommands;
using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;

namespace TK302FBPrinter.Device.Operations
{
    public class SingleCommandOperation<TDeviceCommand> : Operation, INoParamsOperation
        where TDeviceCommand : INoParamsCommand
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly TDeviceCommand _deviceCommand;

        public SingleCommandOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            TDeviceCommand deviceCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _deviceCommand = deviceCommand;
        }

        public bool Execute()
        {
            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_deviceCommand.Execute())
            {
                AddErrorDescription(_deviceCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            Disconnect();
            return true;
        }

        private void Disconnect()
        {
            if (!_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
            }
        }
    }
}
