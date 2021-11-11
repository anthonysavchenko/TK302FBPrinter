using TK302FBPrinter.Configuration;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Printer
{
    public interface IPrinterConnector
    {
        string GetErrorDescription();

        bool Beep(PrinterOptions printerOptions);

        bool ShiftOpen(PrinterOptions printerOptions);

        bool ShiftClose(PrinterOptions printerOptions);

        bool PrintReceipt(PrinterOptions printerOptions, ReceiptDto receipt);
    }
}
