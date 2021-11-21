using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Business.Operations.GetStatusOperation
{
    public interface IGetStatusOperation : IOperation
    {
        bool Execute(out PrinterStatusDto status);
    }
}
