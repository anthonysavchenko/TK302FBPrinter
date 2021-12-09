namespace TK302FBPrinter.Device.Commands.TextDocTextPrint
{
    public interface ITextDocTextPrintCommand : IDeviceCommand
    {
        bool Execute(string text);
    }
}
