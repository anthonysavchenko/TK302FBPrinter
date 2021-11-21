using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.Operations.GetStatusOperation
{
    public interface IGetStatusOperation : IOperation
    {
        bool Execute(out PrinterStatusDto status);
    }
}
