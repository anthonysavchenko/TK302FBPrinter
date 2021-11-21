using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.PrinterStatusGet
{
    public class PrinterStatusGetMockCommand : DeviceCommand, IPrinterStatusGetCommand
    {
        public PrinterStatusGetMockCommand() : base(null) {}

        public bool Execute(out PrinterStatusDto status)
        {
            status = null;
            return true;
        }
    }
}
