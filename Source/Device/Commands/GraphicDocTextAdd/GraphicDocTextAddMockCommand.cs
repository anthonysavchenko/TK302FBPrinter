namespace TK302FBPrinter.Device.Commands.GraphicDocTextAdd
{
    public class GraphicDocTextAddMockCommand : DeviceCommand, IGraphicDocTextAddCommand
    {
        public GraphicDocTextAddMockCommand() : base(null, null) {}

        public bool Execute(
            string text,
            int rotation = 2,
            int positionX = 1,
            int positionY = 1,
            int fontSize = 3,
            int scaleX = 1,
            int scaleY = 1,
            int fontStyle = 11)
        {
            return true;
        }        
    }
}
