namespace TK302FBPrinter.Device.Commands.GraphicDocOpen
{
    public class GraphicDocOpenMockCommand : DeviceCommand, IGraphicDocOpenCommand
    {
        public GraphicDocOpenMockCommand() : base(null, null) {}

        public bool Execute(int xSize, int ySize)
        {
            return true;
        }
    }
}
