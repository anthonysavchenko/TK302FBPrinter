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
    }
}