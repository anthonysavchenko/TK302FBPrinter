using ShiftCloseModel = TK302FBPrinter.Business.Models.ShiftClose;

namespace TK302FBPrinter.Business.Operations.ShiftClose
{
    public interface IShiftCloseOperation : IOperation
    {
        bool Execute(ShiftCloseModel shiftClose);
    }    
}
