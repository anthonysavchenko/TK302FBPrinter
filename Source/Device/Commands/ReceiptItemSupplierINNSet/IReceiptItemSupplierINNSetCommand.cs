namespace TK302FBPrinter.Device.Commands.ReceiptItemSupplierINNSet
{
    public interface IReceiptItemSupplierINNSetCommand : IDeviceCommand
    {
        bool Execute(string supplierINN);
    }
}
