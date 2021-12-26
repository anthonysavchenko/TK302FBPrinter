using ShiftOpenModel = TK302FBPrinter.Business.Models.ShiftOpen;

namespace TK302FBPrinter.Business.Operations.ShiftOpen
{
    public interface IShiftOpenOperation : IOperation
    {
        bool Execute(ShiftOpenModel shiftOpen);
    }
}
