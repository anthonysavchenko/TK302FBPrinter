using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Business.Operations.PrintSlip
{
    public interface IPrintSlipOperation : IOperation
    {
        bool Execute(SlipDto slip);
    }
}
