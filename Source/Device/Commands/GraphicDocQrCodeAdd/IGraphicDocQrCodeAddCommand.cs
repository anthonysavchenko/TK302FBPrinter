namespace TK302FBPrinter.Device.Commands.GraphicDocQrCodeAdd
{
    public interface IGraphicDocQrCodeAddCommand : IDeviceCommand
    {
        bool Execute(string text, int rotation, int positionX, int positionY, int scale);
    }
}
