using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Cut;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ShiftClose;
using ShiftCloseModel = TK302FBPrinter.Business.Models.ShiftClose;

namespace TK302FBPrinter.Business.Operations.ShiftClose
{
    public class ShiftCloseOperation : Operation, IShiftCloseOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IShiftCloseCommand _shiftCloseCommand;
        private readonly ICutCommand _cutCommand;

        public ShiftCloseOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IShiftCloseCommand shiftCloseCommand,
            ICutCommand cutCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _shiftCloseCommand = shiftCloseCommand;
            _cutCommand = cutCommand;
        }

        public bool Execute(ShiftCloseModel shiftClose)
        {
            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_shiftCloseCommand.Execute())
            {
                AddErrorDescription(_shiftCloseCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            if (shiftClose.Cut && !_cutCommand.Execute())
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
