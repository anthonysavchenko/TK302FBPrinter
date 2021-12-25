using System;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ReceiptItemAgentFlagSet
{
    public class ReceiptItemAgentFlagSetCommand : DeviceCommand, IReceiptItemAgentFlagSetCommand
    {
        public ReceiptItemAgentFlagSetCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute()
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.SendOFDData(
                    operatorPassword: _deviceConfig.OperatorPassword,
                    dataType: OFDDataTypeEnum.FlagOfAgentPaymentSubject,
                    dataLength: 1,
                    data: new byte[] { 0x0040 });

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
