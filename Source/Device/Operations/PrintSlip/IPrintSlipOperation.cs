namespace TK302FBPrinter.Device.Operations.PrintSlip
{
    public interface IPrintSlipOperation : IOperation
    {
        bool Execute(string content);
    }
}
