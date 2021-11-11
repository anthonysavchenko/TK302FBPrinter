using TK302FBPrinter.Configuration;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Printer
{
    public class MockPrinterConnector: IPrinterConnector
    {
        public string GetErrorDescription()
        {
            return string.Empty;
        }

        public bool Beep(PrinterOptions printerOptions)
        {
            return true;
        }

        public bool ShiftOpen(PrinterOptions printerOptions)
        {
            return true;
        }

        public bool ShiftClose(PrinterOptions printerOptions)
        {
            return true;
        }

        public bool PrintReceipt(PrinterOptions printerOptions, ReceiptDto receipt)
        {
            return true;
        }
    }
}