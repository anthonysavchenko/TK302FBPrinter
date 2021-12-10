namespace TK302FBPrinter.Device.Commands.GraphicDocBitmapAdd
{
    public class GraphicDocBitmapAddMockCommand : DeviceCommand, IGraphicDocBitmapAddCommand
    {
        public GraphicDocBitmapAddMockCommand() : base(null, null) {}

        public bool Execute(int bitmapId, int rotation, int positionX, int positionY, int scaleX, int scaleY)
        {
            return true;
        }
    }
}
