namespace TK302FBPrinter.Device.Commands.ReceiptItemSupplierINNSet
{
    public class ReceiptItemSupplierINNSetMockCommand : DeviceCommand, IReceiptItemSupplierINNSetCommand
    {
        public ReceiptItemSupplierINNSetMockCommand() : base(null, null) {}

        public bool Execute(string supplierINN)
        {
            return true;
        }
    }
}
