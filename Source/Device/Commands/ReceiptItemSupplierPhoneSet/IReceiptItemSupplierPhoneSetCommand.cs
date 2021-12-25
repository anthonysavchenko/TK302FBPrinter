namespace TK302FBPrinter.Device.Commands.ReceiptItemSupplierPhoneSet
{
    public interface IReceiptItemSupplierPhoneSetCommand : IDeviceCommand
    {
        bool Execute(string supplierPhone);
    }
}
