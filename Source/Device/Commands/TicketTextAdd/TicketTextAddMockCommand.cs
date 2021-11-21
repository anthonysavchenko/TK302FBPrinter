namespace TK302FBPrinter.Device.Commands.TicketTextAdd
{
    public class TicketTextAddMockCommand : DeviceCommand, ITicketTextAddCommand
    {
        public TicketTextAddMockCommand() : base(null, null) {}

        public bool Execute(string text, int xPosition, int yPosition)
        {
            return true;
        }        
    }
}
