namespace TK302FBPrinter.Business.Operations.PrintSlip
{
    public interface IPrintSlipOperation : IOperation
    {
        bool Execute(string content);
    }
}
