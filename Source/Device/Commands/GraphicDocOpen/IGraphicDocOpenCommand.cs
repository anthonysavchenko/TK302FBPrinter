namespace TK302FBPrinter.Device.Commands.GraphicDocOpen
{
    public interface IGraphicDocOpenCommand : IDeviceCommand
    {
        bool Execute(int sizeX, int sizeY);
    }
}
