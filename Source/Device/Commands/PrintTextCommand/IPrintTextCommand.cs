namespace TK302FBPrinter.Device.Commands.PrintTextCommand
{
    public interface IPrintTextCommand : IDeviceCommand
    {
        bool Execute(string text);
    }
}
