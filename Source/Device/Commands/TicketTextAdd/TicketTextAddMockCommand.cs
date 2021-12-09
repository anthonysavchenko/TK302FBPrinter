using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.TicketTextAdd
{
    public class TicketTextAddMockCommand : DeviceCommand, ITicketTextAddCommand
    {
        public TicketTextAddMockCommand() : base(null, null) {}

        public bool Execute(
            string text,
            int rotation = 2,
            int positionX = 1,
            int positionY = 1,
            int fontSize = 3,
            int scaleX = 1,
            int scaleY = 1,
            int fontStyle = 11)
        {
            return true;
        }        
    }
}
