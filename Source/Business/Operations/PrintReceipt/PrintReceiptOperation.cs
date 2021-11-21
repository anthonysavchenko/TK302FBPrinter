using TK302FBPrinter.Device.Commands.CancelLastItem;
using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ReceiptAddItem;
using TK302FBPrinter.Device.Commands.ReceiptCancel;
using TK302FBPrinter.Device.Commands.ReceiptClose;
using TK302FBPrinter.Device.Commands.ReceiptOpen;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Business.Operations.PrintReceipt
{
    public class PrintReceiptOperation : BasePrintReceiptOperation, IPrintReceiptOperation
    {
        public PrintReceiptOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IReceiptOpenCommand receiptOpenCommand,
            IReceiptCloseCommand receiptCloseCommand,
            IReceiptAddItemCommand receiptAddItemCommand,
            IReceiptCancelCommand receiptCancelCommand,
            ICancelLastItemCommand cancelLastItemCommand)
                : base(
                    connectCommand,
                    disconnectCommand,
                    receiptOpenCommand,
                    receiptCloseCommand,
                    receiptAddItemCommand,
                    receiptCancelCommand,
                    cancelLastItemCommand) {}

        public bool Execute(ReceiptDto receipt)
        {
            return base.Execute(receipt);
        }
    }
}
