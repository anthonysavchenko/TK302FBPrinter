namespace TK302FBPrinter.Device.Commands.GraphicDocClose
{
    public interface IGraphicDocCloseCommand : IDeviceCommand
    {
        bool Execute(bool cut);
    }
}
