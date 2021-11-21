using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ShiftClose;

namespace TK302FBPrinter.Business.Operations.ShiftClose
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