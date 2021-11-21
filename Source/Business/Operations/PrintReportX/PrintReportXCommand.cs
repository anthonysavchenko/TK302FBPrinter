using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ReportXPrint;

namespace TK302FBPrinter.Business.Operations.PrintReportX
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
