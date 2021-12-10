namespace TK302FBPrinter.Device.Commands.GraphicDocLineAdd
{
    public class GraphicDocLineAddMockCommand : DeviceCommand, IGraphicDocLineAddCommand
    {
        public GraphicDocLineAddMockCommand() : base(null, null) {}

        public bool Execute(int positionX1, int positionY1, int positionX2, int positionY2, int width)
        {
            return true;
        }
    }
}
