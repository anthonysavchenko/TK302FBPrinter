namespace TK302FBPrinter.Device.Commands
{
    public interface INoParamsCommand : IDeviceCommand
    {
        bool Execute();
    }
}
