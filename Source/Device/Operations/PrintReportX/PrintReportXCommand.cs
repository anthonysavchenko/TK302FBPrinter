using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;
using TK302FBPrinter.Device.DeviceCommands.ReportXPrint;

namespace TK302FBPrinter.Device.Operations.PrintReportX
{
    public class PrintReportXOperation : SingleCommandOperation<IReportXPrintCommand>, IPrintReportXOperation
    {
        public PrintReportXOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IReportXPrintCommand reportXPrintCommand)
                : base(connectCommand, disconnectCommand, reportXPrintCommand) {}
    }
}
