using TK302FBPrinter.Business.Models;

namespace TK302FBPrinter.Business.Operations.PrintReportX
{
    public interface IPrintReportXOperation : IOperation
    {
        bool Execute(ReportX reportX);
    }    
}
