namespace TK302FBPrinter.Device.Commands.TicketTextAdd
{
    public interface ITicketTextAddCommand : IDeviceCommand
    {
        bool Execute(string text, int xPosition, int yPosition);
    }
}
