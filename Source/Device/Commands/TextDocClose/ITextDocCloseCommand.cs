namespace TK302FBPrinter.Device.Commands.TextDocClose
{
    public interface ITextDocCloseCommand : IDeviceCommand
    {
        bool Execute(bool cut);
    }
}
