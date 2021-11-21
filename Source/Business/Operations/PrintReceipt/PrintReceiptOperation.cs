using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ReceiptItemAdd;
using TK302FBPrinter.Device.Commands.ReceiptItemCancel;
using TK302FBPrinter.Device.Commands.ReceiptCancel;
using TK302FBPrinter.Device.Commands.ReceiptClose;
using TK302FBPrinter.Device.Commands.ReceiptOpen;
using TK302FBPrinter.Dto;

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

        public PrintReceiptOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IReceiptOpenCommand receiptOpenCommand,
            IReceiptCloseCommand receiptCloseCommand,
            IReceiptItemAddCommand receiptItemAddCommand,
            IReceiptCancelCommand receiptCancelCommand,
            IReceiptItemCancelCommand receiptItemCancelCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _receiptOpenCommand = receiptOpenCommand;
            _receiptCloseCommand = receiptCloseCommand;
            _receiptItemAddCommand = receiptItemAddCommand;
            _receiptCancelCommand = receiptCancelCommand;
            _receiptItemCancelCommand = receiptItemCancelCommand;            
        }

        public bool Execute(ReceiptDto receipt)
        {
            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_receiptOpenCommand.Execute(receipt))
            {
                AddErrorDescription(_receiptOpenCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            for (int i = 0; i < receipt.Items.Count; i++)
            {
                if (!_receiptItemAddCommand.Execute(receipt.Items[i], receipt.IsReturn))
                {
                    AddErrorDescription(_receiptItemAddCommand.ErrorDescription);
                    RemoveItems(i - 1);
                    CancelReceipt();
                    Disconnect();
                    return false;
                }
            }

            if (!_receiptCloseCommand.Execute(receipt))
            {
                AddErrorDescription(_receiptCloseCommand.ErrorDescription);
                RemoveItems(receipt.Items.Count - 1);
                CancelReceipt();
                Disconnect();
                return false;
            }

            Disconnect();
            return true;
        }

        private void Disconnect()
        {
            if (!_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
            }
        }

        private void RemoveItems(int tillIndex)
        {
            for (int i = 0; i < tillIndex; i++)
            {
                if (!_receiptItemCancelCommand.Execute())
                {
                    AddErrorDescription(_receiptItemCancelCommand.ErrorDescription);
                }
            }
        }

        private void CancelReceipt()
        {
            if (!_receiptCancelCommand.Execute())
            {
                AddErrorDescription(_receiptCancelCommand.ErrorDescription);
            }
        }
    }
}
