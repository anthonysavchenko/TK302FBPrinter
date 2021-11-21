namespace TK302FBPrinter.Device.Commands.TextDocOpen
{
    public class TextDocOpenMockCommand : DeviceCommand, ITextDocOpenCommand
    {
        public TextDocOpenMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}