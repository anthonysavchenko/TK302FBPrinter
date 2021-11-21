namespace TK302FBPrinter.Device.Commands.TextPrintCommand
{
    public class TextPrintMockCommand : DeviceCommand, ITextPrintCommand
    {
        public TextPrintMockCommand() : base(null, null) {}
        
        public bool Execute(string text)
        {
            return true;
        }
    }
}
