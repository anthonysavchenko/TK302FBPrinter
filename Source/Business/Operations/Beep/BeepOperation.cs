using TK302FBPrinter.Device.Commands.Beep;
using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;

namespace TK302FBPrinter.Business.Operations.Beep
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
