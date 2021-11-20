using TK302FBPrinter.Device.DeviceCommands.CancelLastItem;
using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;
using TK302FBPrinter.Device.DeviceCommands.ReceiptAddItem;
using TK302FBPrinter.Device.DeviceCommands.ReceiptCancel;
using TK302FBPrinter.Device.DeviceCommands.ReceiptClose;
using TK302FBPrinter.Device.DeviceCommands.ReceiptOpen;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Operations
{
    public class BasePrintReceiptOperation : Operation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IReceiptOpenCommand _receiptOpenCommand;
        private readonly IReceiptCloseCommand _receiptCloseCommand;
        private readonly IReceiptAddItemCommand _receiptAddItemCommand;
        private readonly IReceiptCancelCommand _receiptCancelCommand;
        private readonly ICancelLastItemCommand _cancelLastItemCommand;

        public BasePrintReceiptOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IReceiptOpenCommand receiptOpenCommand,
            IReceiptCloseCommand receiptCloseCommand,
            IReceiptAddItemCommand receiptAddItemCommand,
            IReceiptCancelCommand receiptCancelCommand,
            ICancelLastItemCommand cancelLastItemCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _receiptOpenCommand = receiptOpenCommand;
            _receiptCloseCommand = receiptCloseCommand;
            _receiptAddItemCommand = receiptAddItemCommand;
            _receiptCancelCommand = receiptCancelCommand;
            _cancelLastItemCommand = cancelLastItemCommand;
        }

        public bool Execute(ReceiptDto receipt, bool isReceiptReturn = false)
        {
            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_receiptOpenCommand.Execute(receipt, isReceiptReturn))
            {
                AddErrorDescription(_receiptOpenCommand.ErrorDescription);
                Disconnect();
                return false;
            }

            for (int i = 0; i < receipt.Items.Count; i++)
            {
                if (!_receiptAddItemCommand.Execute(receipt.Items[i], isReceiptReturn))
                {
                    AddErrorDescription(_receiptAddItemCommand.ErrorDescription);
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
                if (!_cancelLastItemCommand.Execute())
                {
                    AddErrorDescription(_cancelLastItemCommand.ErrorDescription);
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
