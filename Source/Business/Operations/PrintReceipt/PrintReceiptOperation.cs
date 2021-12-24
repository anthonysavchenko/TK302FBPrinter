using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ReceiptItemAdd;
using TK302FBPrinter.Device.Commands.ReceiptItemCancel;
using TK302FBPrinter.Device.Commands.ReceiptCancel;
using TK302FBPrinter.Device.Commands.ReceiptClose;
using TK302FBPrinter.Device.Commands.ReceiptOpen;
using TK302FBPrinter.Business.Models;
using TK302FBPrinter.Device.Commands.Cut;

namespace TK302FBPrinter.Business.Operations.PrintReceipt
{
    public class PrintReceiptOperation : Operation, IPrintReceiptOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IReceiptOpenCommand _receiptOpenCommand;
        private readonly IReceiptCloseCommand _receiptCloseCommand;
        private readonly IReceiptItemAddCommand _receiptItemAddCommand;
        private readonly IReceiptCancelCommand _receiptCancelCommand;
        private readonly IReceiptItemCancelCommand _receiptItemCancelCommand;
        private readonly ICutCommand _cutCommand;

        public PrintReceiptOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IReceiptOpenCommand receiptOpenCommand,
            IReceiptCloseCommand receiptCloseCommand,
            IReceiptItemAddCommand receiptItemAddCommand,
            IReceiptCancelCommand receiptCancelCommand,
            IReceiptItemCancelCommand receiptItemCancelCommand,
            ICutCommand cutCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _receiptOpenCommand = receiptOpenCommand;
            _receiptCloseCommand = receiptCloseCommand;
            _receiptItemAddCommand = receiptItemAddCommand;
            _receiptCancelCommand = receiptCancelCommand;
            _receiptItemCancelCommand = receiptItemCancelCommand;            
            _cutCommand = cutCommand;            
        }

        public bool Execute(Receipt receipt)
        {
            if (receipt.WithConnection && !_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_receiptOpenCommand.Execute(receipt.IsReturn, (int)receipt.Tax))
            {
                AddErrorDescription(_receiptOpenCommand.ErrorDescription);
                Disconnect(receipt.WithConnection);
                return false;
            }

            foreach (var item in receipt.Items)
            {
                if (!_receiptItemAddCommand.Execute(
                    item.Description,
                    item.Quantity,
                    item.Price,
                    (int)item.VAT,
                    receipt.IsReturn))
                {
                    AddErrorDescription(_receiptItemAddCommand.ErrorDescription);
                    CancelReceipt();
                    Disconnect(receipt.WithConnection);
                    return false;
                }
            }

            if (!_receiptCloseCommand.Execute(receipt.Total))
            {
                AddErrorDescription(_receiptCloseCommand.ErrorDescription);
                CancelReceipt();
                Disconnect(receipt.WithConnection);
                return false;
            }

            if (!_cutCommand.Execute())
            {
                AddErrorDescription(_cutCommand.ErrorDescription);
                Disconnect(receipt.WithConnection);
                return false;
            }

            Disconnect(receipt.WithConnection);
            return true;
        }

        private void CancelReceipt()
        {
            if (!_receiptCancelCommand.Execute())
            {
                AddErrorDescription(_receiptCancelCommand.ErrorDescription);
            }
        }

        private void Disconnect(bool withConnection)
        {
            if (withConnection && !_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
            }
        }
    }
}
