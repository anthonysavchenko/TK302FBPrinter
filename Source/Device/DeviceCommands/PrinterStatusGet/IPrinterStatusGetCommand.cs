using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.PrinterStatusGet
{
    public interface IPrinterStatusGetCommand : IDeviceCommand
    {
        bool Execute(out PrinterStatusDto printerStatus);
    }
}
