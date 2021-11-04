namespace TK302FBPrinter.Printer
{
    public class MockPrinterConnector: IPrinterConnector
    {
        public string GetErrorDescription()
        {
            return string.Empty;
        }

        public bool Beep()
        {
            return true;
        }

        public bool ShiftOpen()
        {
            return true;
        }

        public bool ShiftClose()
        {
            return true;
        }

        public bool PrintReceipt()
        {
            return true;
        }
    }
}