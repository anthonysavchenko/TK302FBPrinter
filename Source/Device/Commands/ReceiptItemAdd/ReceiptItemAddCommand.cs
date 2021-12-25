using System;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ReceiptItemAdd
{
    public class ReceiptItemAddCommand : DeviceCommand, IReceiptItemAddCommand
    {
        public ReceiptItemAddCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(string description, long quantity, long price, int vatType, bool isReturn)
        {
            int vat;
            switch (vatType)
            {
                case 1:
                default:
                    vat = 4;
                    break;
                case 2:
                    vat = 3;
                    break;
                case 3:
                    vat = 2;
                    break;
                case 4:
                    vat = 1;
                    break;
                case 5:
                    vat = 6;
                    break;
                case 6:
                    vat = 5;
                    break;
            }
            
            var itemType = !isReturn // Тип расчета - приход
                ? ReceiptItemTypeEnum.Sale
                : ReceiptItemTypeEnum.SaleReturn;

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

            try
            {
                var deviceResponse = _deviceConnector.Connection.PrintRecItem(
                    _deviceConfig.OperatorPassword,

                    // Тип расчета - приход или возврат прихода
                    itemType: !isReturn ? ReceiptItemTypeEnum.Sale : ReceiptItemTypeEnum.SaleReturn,

                    hasDiscountAddon,
                    hasPaymentSubj,
                    hasGoodsAssortment,
                    hasNumberCustomsDeclaration,
                    excludeModifier,
                    paymentWayType: PaymentWayEnum.CompletePayment, // Способ расчета - Полный расчет
                    paymentSubject: PaymentSubjectEnum.RenderedServices, // Предмет расчета - Услуга,
                    codeCountryProducer,
                    quantity: quantity, // Количество
                    amount: price, // Стоимость за единицу
                    excise,
                    deptNumber: vat, // Отдел (НДС)
                    discountAddonType1,
                    discountAddonType2,
                    discountAddonAmount,
                    text: description, // Наименование предмета продажи
                    paymentSubjectText,
                    goodsType,
                    goodsAssortmentGTIN,
                    goodsAssortmentSerial,
                    numberCustomsDeclarationText,
                    hasDiscountAddon2,
                    discountAddonType3,
                    discountAddonType4,
                    discountAddonAmount2);

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
