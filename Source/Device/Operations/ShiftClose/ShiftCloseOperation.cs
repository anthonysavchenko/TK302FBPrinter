using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;
using TK302FBPrinter.Device.DeviceCommands.ShiftClose;

namespace TK302FBPrinter.Device.Operations.ShiftClose
{
    public class ShiftCloseOperation : SingleCommandOperation<IShiftCloseCommand>, IShiftCloseOperation
    {
        public ShiftCloseOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IShiftCloseCommand shiftCloseCommand)
                : base(connectCommand, disconnectCommand, shiftCloseCommand) {}
    }
}