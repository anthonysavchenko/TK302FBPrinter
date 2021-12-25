using System;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ReceiptItemSupplierINNSet
{
    public class ReceiptItemSupplierINNSetCommand : DeviceCommand, IReceiptItemSupplierINNSetCommand
    {
        public ReceiptItemSupplierINNSetCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(string supplierINN)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.SendOFDData(
                    operatorPassword: _deviceConfig.OperatorPassword,
                    dataType: OFDDataTypeEnum.SupplierTIN,
                    data: supplierINN);
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
