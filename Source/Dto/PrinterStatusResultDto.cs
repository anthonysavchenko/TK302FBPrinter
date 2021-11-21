namespace TK302FBPrinter.Dto
{
    public class PrinterStatusResultDto : ExecutionResultDto
    {
        public PrinterStatusResultDto(string errorDescription = null, PrinterStatusDto status = null)
            : base(errorDescription)
        {
            if (!string.IsNullOrEmpty(errorDescription))
            {
                PrinterStatus = null;
                return;
            }
            PrinterStatus = status;
        }

        public PrinterStatusDto PrinterStatus { get; set; }
    }
}
