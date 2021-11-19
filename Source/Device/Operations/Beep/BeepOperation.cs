using TK302FBPrinter.Device.DeviceCommands.Beep;
using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;

namespace TK302FBPrinter.Device.Operations.Beep
{
    public class BeepOperation : SingleCommandOperation<IBeepCommand>, IBeepOperation
    {
        public BeepOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IBeepCommand beepCommand)
                : base(connectCommand, disconnectCommand, beepCommand) {}
    }
}
