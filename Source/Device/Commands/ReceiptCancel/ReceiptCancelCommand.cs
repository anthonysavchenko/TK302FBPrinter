using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ReceiptCancel
{
    public class ReceiptCancelCommand : DeviceCommand, IReceiptCancelCommand
    {
        public ReceiptCancelCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig) {}

        public bool Execute()
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.CheckCancellation(_deviceConfig.OperatorPassword);
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
