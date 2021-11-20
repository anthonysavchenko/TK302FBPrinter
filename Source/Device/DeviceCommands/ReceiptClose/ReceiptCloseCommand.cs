using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.ReceiptClose
{
    public class ReceiptCloseCommand : DeviceCommand, IReceiptCloseCommand
    {
        private readonly PrinterOptions _printerOptions;

        public ReceiptCloseCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute(ReceiptDto receipt)
        {
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

            try
            {
                var deviceResponse = _connector.Connection.CheckClosing(
                    _printerOptions.OperatorPassword,
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
