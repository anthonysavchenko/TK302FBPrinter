namespace TK302FBPrinter.Device.DeviceCommands.PrintTextCommand
{
    public interface IPrintTextCommand : IDeviceCommand
    {
        bool Execute(string text);
    }
}
