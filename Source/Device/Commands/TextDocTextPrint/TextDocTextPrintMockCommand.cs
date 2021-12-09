namespace TK302FBPrinter.Device.Commands.TextDocTextPrint
{
    public class TextDocTextPrintMockCommand : DeviceCommand, ITextDocTextPrintCommand
    {
        public TextDocTextPrintMockCommand() : base(null, null) {}
        
        public bool Execute(string text)
        {
            return true;
        }
    }
}
