namespace TK302FBPrinter.Device.Commands.GraphicDocLineAdd
{
    public interface IGraphicDocLineAddCommand : IDeviceCommand
    {
        bool Execute(int positionX1, int positionY1, int positionX2, int positionY2, int width);
    }
}
