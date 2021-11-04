namespace TK302FBPrinter.Printer
{
    public interface IPrinterConnector
    {
        string GetErrorDescription();

        bool Beep();

        bool ShiftOpen();

        bool ShiftClose();

        bool PrintReceipt();
    }
}