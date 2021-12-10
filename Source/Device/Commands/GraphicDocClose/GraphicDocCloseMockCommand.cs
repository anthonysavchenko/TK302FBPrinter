namespace TK302FBPrinter.Device.Commands.GraphicDocClose
{
    public class GraphicDocCloseMockCommand : DeviceCommand, IGraphicDocCloseCommand
    {
        public GraphicDocCloseMockCommand() : base(null, null) {}

        public bool Execute(bool cut)
        {
            return true;
        }
    }
}
