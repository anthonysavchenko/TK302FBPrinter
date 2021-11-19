using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptOpen
{
    public class ReceiptOpenCommand : DeviceCommand, IReceiptOpenCommand
    {
        private readonly PrinterOptions _printerOptions;
        
        public ReceiptOpenCommand(
            ProtocolAPI connection,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connection)
        {
            _printerOptions = printerOptions.Value;
        }

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

            var deviceResponse =
                _connection.OpenFiscalDocument(_printerOptions.OperatorPassword, print, saveOnFile, docType, taxCode);
            return !CheckRespose(deviceResponse);
        }
    }
}
