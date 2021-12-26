using TK302FBPrinter.Business.Models;
using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Cut;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ReportXPrint;

namespace TK302FBPrinter.Business.Operations.PrintReportX
{
    public class PrintReportXOperation : Operation, IPrintReportXOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IReportXPrintCommand _reportXPrintCommand;
        private readonly ICutCommand _cutCommand;

        public PrintReportXOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IReportXPrintCommand reportXPrintCommand,
            ICutCommand cutCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _reportXPrintCommand = reportXPrintCommand;
            _cutCommand = cutCommand;
        }

        public bool Execute(ReportX reportX)
        {
            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_reportXPrintCommand.Execute())
            {
                AddErrorDescription(_reportXPrintCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            if (reportX.Cut && !_cutCommand.Execute())
            {
                AddErrorDescription(_cutCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            Disconnect();
            return true;
        }
        private void Disconnect()
        {
            if (!_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
            }
        }
    }
}
