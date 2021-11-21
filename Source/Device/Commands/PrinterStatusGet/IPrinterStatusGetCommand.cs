using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.PrinterStatusGet
{
    public interface IPrinterStatusGetCommand : IDeviceCommand
    {
        bool Execute(out PrinterStatusDto printerStatus);
    }
}
