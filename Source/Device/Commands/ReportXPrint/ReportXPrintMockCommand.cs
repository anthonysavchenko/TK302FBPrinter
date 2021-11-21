namespace TK302FBPrinter.Device.Commands.ReportXPrint
{
    public class ReportXPrintMockCommand : DeviceCommand, IReportXPrintCommand
    {
        public ReportXPrintMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
