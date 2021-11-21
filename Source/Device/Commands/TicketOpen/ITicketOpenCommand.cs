namespace TK302FBPrinter.Device.Commands.TicketOpen
{
    public interface ITicketOpenCommand : IDeviceCommand
    {
        bool Execute(int xSize, int ySize);
    }
}
