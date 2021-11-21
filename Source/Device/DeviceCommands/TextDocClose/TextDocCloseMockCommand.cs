namespace TK302FBPrinter.Device.DeviceCommands.TextDocClose
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
