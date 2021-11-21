namespace TK302FBPrinter.Configuration
{
    public class DeviceConfig
    {
        public const string Device = "Device";

        public bool EmulationMode { get; set; } = false;
        
        public string OperatorPassword { get; set; } = string.Empty;

        public string PortName { get; set; } = string.Empty;
    }
}
