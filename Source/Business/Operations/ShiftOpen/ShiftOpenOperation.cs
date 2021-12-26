using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Cut;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ShiftOpen;
using ShiftOpenModel = TK302FBPrinter.Business.Models.ShiftOpen;

namespace TK302FBPrinter.Business.Operations.ShiftOpen
{
    public class ShiftOpenOperation : Operation, IShiftOpenOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IShiftOpenCommand _shiftOpenCommand;
        private readonly ICutCommand _cutCommand;

        public ShiftOpenOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IShiftOpenCommand shifOpenCommand,
            ICutCommand cutCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _shiftOpenCommand = shifOpenCommand;
            _cutCommand = cutCommand;
        }

        public bool Execute(ShiftOpenModel shiftOpen)
        {
            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_shiftOpenCommand.Execute())
            {
                AddErrorDescription(_shiftOpenCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            if (shiftOpen.Cut && !_cutCommand.Execute())
            {
                AddErrorDescription(_cutCommand.ErrorDescription);
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
