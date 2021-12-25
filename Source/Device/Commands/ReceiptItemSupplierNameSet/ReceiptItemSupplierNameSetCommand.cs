using System;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ReceiptItemSupplierNameSet
{
    public class ReceiptItemSupplierNameSetCommand : DeviceCommand, IReceiptItemSupplierNameSetCommand
    {
        public ReceiptItemSupplierNameSetCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(string supplierName)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.SendOFDData(
                    operatorPassword: _deviceConfig.OperatorPassword,
                    dataType: OFDDataTypeEnum.SupplierName,
                    data: supplierName);
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
