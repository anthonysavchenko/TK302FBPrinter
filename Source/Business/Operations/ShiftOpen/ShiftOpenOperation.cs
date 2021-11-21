using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ShiftOpen;

namespace TK302FBPrinter.Business.Operations.ShiftOpen
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
