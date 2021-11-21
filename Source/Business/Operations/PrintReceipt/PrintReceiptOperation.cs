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
    public class PrintReceiptOperation : BasePrintReceiptOperation, IPrintReceiptOperation
    {
        public PrintReceiptOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IReceiptOpenCommand receiptOpenCommand,
            IReceiptCloseCommand receiptCloseCommand,
            IReceiptItemAddCommand receiptItemAddCommand,
            IReceiptCancelCommand receiptCancelCommand,
            IReceiptItemCancelCommand receiptItemCancelCommand)
                : base(
                    connectCommand,
                    disconnectCommand,
                    receiptOpenCommand,
                    receiptCloseCommand,
                    receiptItemAddCommand,
                    receiptCancelCommand,
                    receiptItemCancelCommand) {}

        public bool Execute(ReceiptDto receipt)
        {
            return base.Execute(receipt);
        }
    }
}
