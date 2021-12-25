namespace TK302FBPrinter.Device.Commands.ReceiptItemSupplierNameSet
{
    public interface IReceiptItemSupplierNameSetCommand : IDeviceCommand
    {
        bool Execute(string supplierName);
    }
}
