using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.GraphicDocQrCodeAdd
{
    public class GraphicDocQrCodeAddMockCommand : DeviceCommand, IGraphicDocQrCodeAddCommand
    {
        public GraphicDocQrCodeAddMockCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(string text, int rotation, int positionX, int positionY, int scale)
        {
            return true;
        }
    }
}
