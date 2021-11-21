using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ReportXPrint
{
    public class ReportXPrintCommand : DeviceCommand, IReportXPrintCommand
    {
        public ReportXPrintCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig) {}

        public bool Execute()
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.XReport(
                    _deviceConfig.OperatorPassword,
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
