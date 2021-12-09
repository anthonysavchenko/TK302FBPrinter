namespace TK302FBPrinter.Device.Commands.TicketClose
{
    public class TicketCloseMockCommand : DeviceCommand, ITicketCloseCommand
    {
        public TicketCloseMockCommand() : base(null, null) {}

        public bool Execute(bool cut)
        {
            return true;
        }
    }
}
