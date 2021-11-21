namespace TK302FBPrinter.Device.Commands.TicketOpen
{
    public class TicketOpenMockCommand : DeviceCommand, ITicketOpenCommand
    {
        public TicketOpenMockCommand() : base(null, null) {}

        public bool Execute(int xSize, int ySize)
        {
            return true;
        }
    }
}
