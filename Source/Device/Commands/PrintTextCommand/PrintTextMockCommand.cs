namespace TK302FBPrinter.Device.Commands.PrintTextCommand
{
    public class PrintTextMockCommand : DeviceCommand, IPrintTextCommand
    {
        public PrintTextMockCommand() : base(null, null) {}
        
        public bool Execute(string text)
        {
            return true;
        }
    }
}
