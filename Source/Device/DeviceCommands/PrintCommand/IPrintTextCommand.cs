namespace TK302FBPrinter.Device.DeviceCommands.PrintCommand
{
    public interface IPrintTextCommand : IDeviceCommand
    {
        bool Execute(string text);
    }
}
