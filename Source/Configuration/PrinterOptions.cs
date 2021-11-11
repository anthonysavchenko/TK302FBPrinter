namespace TK302FBPrinter.Configuration
{
    public class PrinterOptions
    {
        public const string Printer = "Printer";

        public bool EmulationMode { get; set; } = true;
        
        public string OperatorPassword { get; set; } = string.Empty;

        public string PortName { get; set; } = string.Empty;
    }
}
