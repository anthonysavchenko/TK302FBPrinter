namespace TK302FBPrinter.Device.Commands.TextDocClose
{
    public class TextDocCloseMockCommand : DeviceCommand, ITextDocCloseCommand
    {
        public TextDocCloseMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
