using TK302FBPrinter.Business.Models;

namespace TK302FBPrinter.Business.Operations.PrintComplexDoc
{
    public interface IPrintComplexDocOperation : IOperation
    {
        bool Execute(ComplexDoc complexDoc);
    }
}
