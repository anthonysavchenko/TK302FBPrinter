namespace TK302FBPrinter.Device.Commands.ReceiptItemSupplierPhoneSet
{
    public class ReceiptItemSupplierPhoneSetMockCommand : DeviceCommand, IReceiptItemSupplierPhoneSetCommand
    {
        public ReceiptItemSupplierPhoneSetMockCommand() : base(null, null) {}

        public bool Execute(string supplierPhone)
        {
            return true;
        }
    }
}
