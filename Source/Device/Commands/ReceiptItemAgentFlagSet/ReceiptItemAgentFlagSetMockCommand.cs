namespace TK302FBPrinter.Device.Commands.ReceiptItemAgentFlagSet
{
    public class ReceiptItemAgentFlagSetMockCommand : DeviceCommand, IReceiptItemAgentFlagSetCommand
    {
        public ReceiptItemAgentFlagSetMockCommand() : base(null, null) {}

        public bool Execute()
        {
            return true;
        }
    }
}
