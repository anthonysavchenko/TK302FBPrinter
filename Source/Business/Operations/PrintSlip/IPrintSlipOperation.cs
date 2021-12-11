using TK302FBPrinter.Business.Models;

namespace TK302FBPrinter.Business.Operations.PrintSlip
{
    public interface IPrintSlipOperation : IOperation
    {
        bool Execute(Slip slip);
    }
}
