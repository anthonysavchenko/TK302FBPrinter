namespace TK302FBPrinter.Device.Commands.GraphicDocOpen
{
    public class GraphicDocOpenMockCommand : DeviceCommand, IGraphicDocOpenCommand
    {
        public GraphicDocOpenMockCommand() : base(null, null) {}

        public bool Execute(int sizeX, int sizeY)
        {
            return true;
        }
    }
}
