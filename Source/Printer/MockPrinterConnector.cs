namespace TK302FBPrinter.Printer
{
    public class MockPrinterConnector: IPrinterConnector
    {
        public bool Beep()
        {
            return true;
        }
    }
}