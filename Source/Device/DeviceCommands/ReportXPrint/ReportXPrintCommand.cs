using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.ReportXPrint
{
    public class ReportXPrintCommand : DeviceCommand, IReportXPrintCommand
    {
        private readonly PrinterOptions _printerOptions;

        public ReportXPrintCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            try
            {
                var deviceResponse = _connector.Connection.XReport(
                    _printerOptions.OperatorPassword,
                    AddCashDrawer: false);

                return CheckRespose(deviceResponse);
            }
            catch (Exception exception)
            {
                AddException(exception);
                return false;
            }
        }
    }
}
