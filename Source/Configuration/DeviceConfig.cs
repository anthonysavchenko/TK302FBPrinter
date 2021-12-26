namespace TK302FBPrinter.Configuration
{
    public class DeviceConfig
    {
        public const string SectionName = "Device";

        public bool EmulationMode { get; set; }
        
        public string OperatorPassword { get; set; }

        public string PortName { get; set; }
    }
}
