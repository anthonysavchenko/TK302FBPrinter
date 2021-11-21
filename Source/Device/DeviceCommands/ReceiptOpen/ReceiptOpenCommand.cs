using System;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptOpen
{
    public class ReceiptOpenCommand : DeviceCommand, IReceiptOpenCommand
    {
        public ReceiptOpenCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig) {}

        public bool Execute(ReceiptDto receipt, bool isReturnReceipt = false)
        {
            bool print = true; // Печатать фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)

            var docType = !isReturnReceipt // Тип документа (приход, возврат и т.д.)
                ? ReceiptTypeEnum.Sale
                : ReceiptTypeEnum.SaleReturn;

            TaxCodeEnum taxCode; // Система налогооблажения (СНО)
            switch (receipt.Tax)
            {
                case TaxType.Traditional:
                    taxCode = TaxCodeEnum.PatentTaxSystem;
                    break;
                case TaxType.LightIncome:
                    taxCode = TaxCodeEnum.LightIncome;
                    break;
                case TaxType.LightIncomeNoExpenses:
                    taxCode = TaxCodeEnum.LightIncomeNoExpenses;
                    break;
                case TaxType.SingleTax:
                    taxCode = TaxCodeEnum.SingleTax;
                    break;
                case TaxType.Agricultural:
                    taxCode = TaxCodeEnum.AgriculturalTax;
                    break;
                case TaxType.Patent:
                    taxCode = TaxCodeEnum.PatentTaxSystem;
                    break;
                case TaxType.AutomaticMode:
                default:
                    taxCode = TaxCodeEnum.AutomaticMode;
                    break;
            }

            try
            {
                var deviceResponse = _deviceConnector.Connection.OpenFiscalDocument(
                    _deviceConfig.OperatorPassword,
                    print,
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
