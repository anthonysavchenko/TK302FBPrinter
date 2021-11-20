namespace TK302FBPrinter.Device.DeviceCommands.TextDocOpen
{
    public class TextDocOpenMockCommand : DeviceCommand, ITextDocOpenCommand
    {
        public TextDocOpenMockCommand() : base(null) {}

        public bool Execute()
        {
            return true;
        }
    }
}