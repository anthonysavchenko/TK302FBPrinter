namespace TK302FBPrinter.Device.DeviceCommands.ReportXPrint
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
