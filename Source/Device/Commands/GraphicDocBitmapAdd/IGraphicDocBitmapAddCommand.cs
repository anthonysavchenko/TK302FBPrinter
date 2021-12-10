namespace TK302FBPrinter.Device.Commands.GraphicDocBitmapAdd
{
    public interface IGraphicDocBitmapAddCommand : IDeviceCommand
    {
        bool Execute(int bitmapId, int rotation, int positionX, int positionY, int scaleX, int scaleY);
    }
}
