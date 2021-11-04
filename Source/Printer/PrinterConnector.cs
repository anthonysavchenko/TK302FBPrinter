using System;
using System.IO.Ports;
using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.Comunication;
using Custom.Fiscal.RUSProtocolAPI.CustomRU;
using Custom.Fiscal.RUSProtocolAPI.Enums;

namespace TK302FBPrinter.Printer
{
    public class PrinterConnector : IPrinterConnector
    {
        private ProtocolAPI _connection;
        private string _errorDescription;

        private bool CheckRespose(APIBaseResponse response)
        {
            if (response.ErrorCode != 0)
            {
                _errorDescription = $"ErrorCode: {response.ErrorCode}. "
                    + "ErrorDescription: {response.ErrorDescription}. OperatorCode: {response.OperatorCode}";
                return false;
            }
            return true;            
        }

        private bool Connect()
        {
            if (_connection != null)
            {
                return true;
            }

            var serialPortParams = new SerialPortParams()
            {
                BaudRate = 57600,
                DataBits = 8,
                HandshakeProp = Handshake.None,
                Parity = Parity.None,
                PortName = "COM3",
                StopBits = StopBits.One,
                Dtr = false,
                Rts = false
            };

            _connection = new ProtocolAPI()
            {
                ComunicationType = ComunicationTypeEnum.RS232,
                ComunicationParams = new object[]
                {
                    serialPortParams.BaudRate,
                    serialPortParams.DataBits,
                    serialPortParams.HandshakeProp,
                    serialPortParams.Parity,
                    serialPortParams.PortName,
                    serialPortParams.StopBits,
                    serialPortParams.Dtr,
                    serialPortParams.Rts
                }
            };

            var printerResponse = _connection.OpenConnection();

            return CheckRespose(printerResponse);
        }

        private bool Disconnect()
        {
            if (_connection == null)
            {
                return true;
            }
            var printerResponse = _connection.CloseConnection();
            return CheckRespose(printerResponse);
        }

        private bool Execute(Func<APIBaseResponse> proccess, bool connect = true, bool disconnect = true)
        {
            if (connect && !Connect())
            {
                return false;
            }

            var printerResponse = proccess();
            if (!CheckRespose(printerResponse))
            {
                return false;
            }

            if (disconnect && !Disconnect())
            {
                return false;
            }

            return true;
        }

        public string GetErrorDescription()
        {
            return _errorDescription;
        }

        public bool Beep()
        {
            var operatorPassword = "999999";

            return Execute(() =>
            {
                return _connection.Beep(operatorPassword);
            });
        }

        public bool ShiftOpen()
        {
            var operatorPassword = "999999";
            bool print = true; // Печатаь фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)

            return Execute(() =>
            {
                return _connection.OpenFiscalDay(operatorPassword, print, saveOnFile);
            });
        }

        public bool ShiftClose()
        {
            var operatorPassword = "999999";
            bool print = true; // Печатаь фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)

            return Execute(() =>
            {
                return _connection.ZReport(operatorPassword, print, saveOnFile);
            });
        }

        public bool PrintReceipt()
        {
            var operatorPassword = "999999";
            bool print = true; // Печатаь фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)
            var docType = ReceiptTypeEnum.Sale; // Тип документа (приход, возврат и т.д.)
            var taxCode = TaxCodeEnum.PatentTaxSystem; // Система налогооблажения (СНО)

            if (!Execute(() =>
                {
                    return _connection.OpenFiscalDocument(
                        operatorPassword,
                        print,
                        saveOnFile,
                        docType,
                        taxCode);
                },
                connect: true,
                disconnect: false))
            {
                return false;
            }

            string text = "Товар для продажи №1"; // Наименование предмета продажи
            long quantity = 1000; // Количество
            long amount = 22000;// Стоимость за единицу
            int deptNumber = 4; // Отдел (НДС)
            
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

            if (!Execute(() =>
                {
                    return _connection.PrintRecItem(
                        operatorPassword,
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
                },
                connect: true,
                disconnect: false))
            {
                return false;
            }

            long amountPaymentCash = 0; // Итого наличными
            long amountPaymentCashless = 0; // Итого безналичными
            long amountPaymentPrepay = 0;// Итого аванс
            long amountPaymentCredit = 0; // Итого кредит
            long amountPaymentOther = 0; // Итого другие типы оплаты
            var hasAdditionalPropertyCheck = false; // Наличие Дополнительного реквизита чека #1192
            var additionalPropertyCheckText = ""; // Значение Дополнительного реквизита чека #1192
            var hasFieldReceiver = false; // Наличие реквизита Получатель/Покупатель #1227
            var receiver = ""; // Значение реквизита Получатель/Покупатель #1227
            var hasFieldReceiverInn = false; // Наличие ИНН Покупателя #1228
            var receiverInn = ""; // Значение ИНН Покупателя #1228
            var subtotalRounding = false; // Округление подытога до целых рублей

            if (!Execute(() =>
                {
                    return _connection.CheckClosing(
                        operatorPassword,
                        amountPaymentCash,
                        amountPaymentCashless,
                        amountPaymentPrepay,
                        amountPaymentCredit,
                        amountPaymentOther,
                        hasAdditionalPropertyCheck,
                        additionalPropertyCheckText,
                        hasFieldReceiver,
                        receiver,
                        hasFieldReceiverInn,
                        receiverInn,
                        subtotalRounding);
                },
                connect: false,
                disconnect: true))
            {
                return false;
            }
            
            return true;
        }
    }
}
