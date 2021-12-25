using System;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ReceiptItemSupplierPhoneSet
{
    public class ReceiptItemSupplierPhoneSetCommand : DeviceCommand, IReceiptItemSupplierPhoneSetCommand
    {
        public ReceiptItemSupplierPhoneSetCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(string supplierPhone)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.SendOFDData(
                    operatorPassword: _deviceConfig.OperatorPassword,
                    dataType: OFDDataTypeEnum.SupplierPhone,
                    data: supplierPhone);
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
