namespace TK302FBPrinter.Device.Commands.TextPrintCommand
{
    public interface ITextPrintCommand : IDeviceCommand
    {
        bool Execute(string text);
    }
}
