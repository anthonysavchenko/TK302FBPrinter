namespace TK302FBPrinter.Device.Commands.TextDocTextAdd
{
    public class TextDocTextAddMockCommand : DeviceCommand, ITextDocTextAddCommand
    {
        public TextDocTextAddMockCommand() : base(null, null) {}
        
        public bool Execute(string text)
        {
            return true;
        }
    }
}
