using System;
using System.Collections.Generic;
using System.IO.Ports;
using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.Comunication;
using Custom.Fiscal.RUSProtocolAPI.CustomRU;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Printer
{
    public class PrinterConnector : IPrinterConnector
    {
        private ProtocolAPI _connection;
        private List<string> _errors = new List<string>();

        private bool CheckRespose(APIBaseResponse response, bool disconnectOnError = true)
        {
            if (response.ErrorCode != 0)
            {
                _errors.Add(
                    $"ErrorCode: {response.ErrorCode}. "
                        + $"ErrorDescription: {response.ErrorDescription}. "
                        + $"OperatorCode: {response.OperatorCode}");
                if (disconnectOnError)
                {
                    Disconnect();
                }
                return false;
            }
            return true;            
        }

        private bool Connect(PrinterOptions printerOptions)
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
                PortName = printerOptions.PortName,
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

            return CheckRespose(printerResponse, disconnectOnError: false);
        }

        private bool Disconnect()
        {
            if (_connection == null)
            {
                return true;
            }
            var printerResponse = _connection.CloseConnection();
            return CheckRespose(printerResponse, disconnectOnError: false);
        }

        private bool Execute(
            Func<APIBaseResponse> proccess,
            bool connect = true,
            PrinterOptions printerOptions = null,
            bool disconnect = true)
        {
            if (connect && !Connect(printerOptions))
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
            if (_errors.Count == 0)
            {
                return string.Empty;
            }

            if (_errors.Count == 1)
            {
                return $"{_errors[0]}";
            }

            string errorDescription = $"Errors quantity: {_errors.Count}";

            for (var i = 0; i < _errors.Count; i++)
            {
                if (i > 0)
                {
                    errorDescription += "\r\n";
                }
                errorDescription += $"{i + 1}. ${_errors[i]}";
            }

            return errorDescription;
        }

        public bool Beep(PrinterOptions printerOptions)
        {
            return Execute(() =>
            {
                return _connection.Beep(printerOptions.OperatorPassword);
            },
            printerOptions: printerOptions);
        }

        public bool ShiftOpen(PrinterOptions printerOptions)
        {
            bool print = true; // Печатаь фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)

            return Execute(() =>
            {
                return _connection.OpenFiscalDay(printerOptions.OperatorPassword, print, saveOnFile);
            },
            printerOptions: printerOptions);
        }

        public bool ShiftClose(PrinterOptions printerOptions)
        {
            bool print = true; // Печатаь фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)

            return Execute(() =>
            {
                return _connection.ZReport(printerOptions.OperatorPassword, print, saveOnFile);
            },
            printerOptions: printerOptions);
        }

        public bool PrintReceipt(PrinterOptions printerOptions, ReceiptDto receipt)
        {
            bool print = true; // Печатать фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)
            var docType = ReceiptTypeEnum.Sale; // Тип документа (приход, возврат и т.д.)
            
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

            if (!Execute(() =>
                {
                    return _connection.OpenFiscalDocument(
                        printerOptions.OperatorPassword,
                        print,
                        saveOnFile,
                        docType,
                        taxCode);
                },
                connect: true,
                printerOptions,
                disconnect: false))
            {
                return false;
            }

            foreach (var item in receipt.Items)
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
                    case VATType.Percent10from110:
                        deptNumber = 6;
                        break;
                    case VATType.Percent20from120:
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

                if (!Execute(() =>
                    {
                        return _connection.PrintRecItem(
                            printerOptions.OperatorPassword,
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
                    connect: false,
                    disconnect: false))
                {
                    return false;
                }
            }

            long amountPaymentCash = 0; // Итого наличными
            long amountPaymentCashless = receipt.Total; // Итого безналичными
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
                        printerOptions.OperatorPassword,
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
