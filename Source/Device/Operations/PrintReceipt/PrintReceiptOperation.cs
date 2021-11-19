using TK302FBPrinter.Device.DeviceCommands.CancelLastItem;
using TK302FBPrinter.Device.DeviceCommands.Connect;
using TK302FBPrinter.Device.DeviceCommands.Disconnect;
using TK302FBPrinter.Device.DeviceCommands.ReceiptAddItem;
using TK302FBPrinter.Device.DeviceCommands.ReceiptCancel;
using TK302FBPrinter.Device.DeviceCommands.ReceiptClose;
using TK302FBPrinter.Device.DeviceCommands.ReceiptOpen;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Operations.PrintReceipt
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
