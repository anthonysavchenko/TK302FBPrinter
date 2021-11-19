namespace TK302FBPrinter.Device.DeviceCommands.PrintCommand
{
    public class PrintTextMockCommand : DeviceCommand, IPrintTextCommand
    {
        public PrintTextMockCommand() : base(null) {}
        
        public bool Execute(string text)
        {
            return true;
        }
    }
}
