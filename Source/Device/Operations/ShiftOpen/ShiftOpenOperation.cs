using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;
using TK302FBPrinter.Device.DeviceCommands.ShiftOpen;

namespace TK302FBPrinter.Device.Operations.ShiftOpen
{
    public class ShiftOpenOperation : SingleCommandOperation<IShiftOpenCommand>, IShiftOpenOperation
    {
        public ShiftOpenOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IShiftOpenCommand shiftOpenCommand)
                : base(connectCommand, disconnectCommand, shiftOpenCommand) {}
    }
}
