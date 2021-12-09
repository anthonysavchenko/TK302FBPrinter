namespace TK302FBPrinter.Device.Commands.TextDocTextAdd
{
    public interface ITextDocTextAddCommand : IDeviceCommand
    {
        bool Execute(string text);
    }
}
