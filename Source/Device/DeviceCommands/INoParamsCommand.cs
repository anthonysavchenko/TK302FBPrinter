namespace TK302FBPrinter.Device.DeviceCommands
{
    public interface INoParamsCommand : IDeviceCommand
    {
        bool Execute();
    }
}
