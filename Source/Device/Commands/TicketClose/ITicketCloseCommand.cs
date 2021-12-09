namespace TK302FBPrinter.Device.Commands.TicketClose
{
    public interface ITicketCloseCommand : IDeviceCommand
    {
        bool Execute(bool cut);
    }
}
