using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ReceiptClose
{
    public class ReceiptCloseCommand : DeviceCommand, IReceiptCloseCommand
    {
        public ReceiptCloseCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(int total)
        {
            long amountPaymentCash = 0; // Итого наличными
            long amountPaymentCashless = total; // Итого безналичными
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
                var deviceResponse = _deviceConnector.Connection.CheckClosing(
                    _deviceConfig.OperatorPassword,
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
