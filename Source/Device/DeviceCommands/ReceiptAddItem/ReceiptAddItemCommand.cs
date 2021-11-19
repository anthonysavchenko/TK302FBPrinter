using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptAddItem
{
    public class ReceiptAddItemCommand : DeviceCommand, IReceiptAddItemCommand
    {
        private readonly PrinterOptions _printerOptions;

        public ReceiptAddItemCommand(
            ProtocolAPI connection,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connection)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute(ReceiptItemDto item)
        {
            string text = item.Description; // Наименование предмета продажи
            long quantity = item.Quantity; // Количество
            long amount = item.Price; // Стоимость за единицу
            
            int deptNumber; // Отдел (НДС)

            switch (item.VAT)
            {
                case VATType.Percent0:
                    deptNumber = 3;
                    break;
                case VATType.Percent10:
                    deptNumber = 2;
                    break;
                case VATType.Percent20:
                    deptNumber = 1;
                    break;
                case VATType.Percent10Base110:
                    deptNumber = 6;
                    break;
                case VATType.Percent20Base120:
                    deptNumber = 5;
                    break;
                case VATType.NoVAT:
                default:
                    deptNumber = 4;
                    break;
            }
            
            var itemType = ReceiptItemTypeEnum.Sale; // Тип расчета - приход

            var paymentSubject = PaymentSubjectEnum.GoodsForSelling; // Товар
            var paymentWayType = PaymentWayEnum.CompletePayment; // Полный расчет
            long excise = 0; // Акциз
            bool hasPaymentSubj = false; // Нет тега #1191 - Дополнительный реквизит предмета продажи
            string paymentSubjectText = "";
            bool hasNumberCustomsDeclaration = false; // Наличие кода таможенной декларации (#1231)
            bool excludeModifier = false; // Исключить позицию из скидки на подытог (не применять к этой позиции скидку, при подсчете подытога)
            string codeCountryProducer = "000"; // Код страны производителя

            bool hasDiscountAddon = false; // Наличие скидки/наценки №1
            int discountAddonType1 = 0; // Тип скидки/наценки. 0 - скидка, 1 - наценка
            int discountAddonType2 = 0; // Тип скидки/наценки. 0 - сумма, 1 - процент
            int discountAddonAmount = 0; // Сумма(%) скидки/наценки №1

            bool hasDiscountAddon2 = false; // Наличие скидки/наценки №2
            var discountAddonType3 = 0;// Тип скидки/наценки №2. 0 - скидка, 1 - наценка    
            var discountAddonType4 = 0; // Тип скидки/наценки №2. 0 - сумма, 1 - процент  
            var discountAddonAmount2 = 0; // Сумма(%) скидки/наценки №2    

            bool hasGoodsAssortment = false; // Нет тега #1162 - Код маркировки (для ФФД 1.05, 1.1)
            var goodsType = CodeOfGoodsEnum.tobacco; // Тип кода маркировки. Игнорируется, если hasGoodsAssortment = false
            var goodsAssortmentGTIN = ""; // GTIN кода маркировки. Игнорируется, если hasGoodsAssortment = false
            var goodsAssortmentSerial = ""; // Серийный номер кода маркировки. Игнорируется, если hasGoodsAssortment = false
            var numberCustomsDeclarationText = ""; // Номер дакларации. Игнорируется, если hasGoodsAssortment = false

            var deviceResponse = _connection.PrintRecItem(
                _printerOptions.OperatorPassword,
                itemType,
                hasDiscountAddon,
                hasPaymentSubj,
                hasGoodsAssortment,
                hasNumberCustomsDeclaration,
                excludeModifier,
                paymentWayType,
                paymentSubject,
                codeCountryProducer,
                quantity,
                amount,
                excise,
                deptNumber,
                discountAddonType1,
                discountAddonType2,
                discountAddonAmount,
                text,
                paymentSubjectText,
                goodsType,
                goodsAssortmentGTIN,
                goodsAssortmentSerial,
                numberCustomsDeclarationText,
                hasDiscountAddon2,
                discountAddonType3,
                discountAddonType4,
                discountAddonAmount2);

            return !CheckRespose(deviceResponse);
        }
    }
}
