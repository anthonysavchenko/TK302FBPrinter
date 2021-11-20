namespace TK302FBPrinter.Device.DeviceCommands.TextDocClose
{
    public class TextDocCloseMockCommand : DeviceCommand, ITextDocCloseCommand
    {
        public TextDocCloseMockCommand() : base(null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
