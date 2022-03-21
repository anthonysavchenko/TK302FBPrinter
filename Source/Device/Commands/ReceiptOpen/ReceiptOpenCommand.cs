using System;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ReceiptOpen
{
    public class ReceiptOpenCommand : DeviceCommand, IReceiptOpenCommand
    {
        public ReceiptOpenCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(bool isReturn, int taxType, bool print)
        {
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)

            // Тип документа (приход, возврат и т.д.)
            var docType = !isReturn ? ReceiptTypeEnum.Sale : ReceiptTypeEnum.SaleReturn;

            TaxCodeEnum taxCode; // Система налогооблажения (СНО)
            switch (taxType)
            {
                case 0:
                default:
                    taxCode = TaxCodeEnum.AutomaticMode;
                    break;
                case 1:
                    taxCode = TaxCodeEnum.Traditional;
                    break;
                case 2:
                    taxCode = TaxCodeEnum.LightIncome;
                    break;
                case 3:
                    taxCode = TaxCodeEnum.LightIncomeNoExpenses;
                    break;
                case 4:
                    taxCode = TaxCodeEnum.SingleTax;
                    break;
                case 5:
                    taxCode = TaxCodeEnum.AgriculturalTax;
                    break;
                case 6:
                    taxCode = TaxCodeEnum.PatentTaxSystem;
                    break;
            }

            try
            {
                var deviceResponse = _deviceConnector.Connection.OpenFiscalDocument(
                    _deviceConfig.OperatorPassword,
                    print: print, // Печатать фискальный документ (ФД)
                    saveOnFile,
                    docType,
                    taxCode);

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
