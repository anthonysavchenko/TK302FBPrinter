namespace TK302FBPrinter.Device.Commands.ReceiptItemSupplierNameSet
{
    public class ReceiptItemSupplierNameSetMockCommand : DeviceCommand, IReceiptItemSupplierNameSetCommand
    {
        public ReceiptItemSupplierNameSetMockCommand() : base(null, null) {}

        public bool Execute(string supplierName)
        {
            return true;
        }
    }
}
