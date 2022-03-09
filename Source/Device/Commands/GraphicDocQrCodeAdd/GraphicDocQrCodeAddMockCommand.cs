namespace TK302FBPrinter.Device.Commands.GraphicDocQrCodeAdd
{
    public class GraphicDocQrCodeAddMockCommand : DeviceCommand, IGraphicDocQrCodeAddCommand
    {
        public GraphicDocQrCodeAddMockCommand() : base(null, null) {}

        public bool Execute(string text, int rotation, int positionX, int positionY, int scale)
        {
            return true;
        }
    }
}
