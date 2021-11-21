using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Commands.PrinterStatusGet
{
    public class PrinterStatusGetMockCommand : DeviceCommand, IPrinterStatusGetCommand
    {
        public PrinterStatusGetMockCommand() : base(null, null) {}

        public bool Execute(out PrinterStatusDto status)
        {
            status = null;
            return true;
        }
    }
}
