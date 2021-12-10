using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.GraphicDocQrCodeAdd
{
    public class GraphicDocQrCodeAddCommand : DeviceCommand, IGraphicDocQrCodeAddCommand
    {
        public GraphicDocQrCodeAddCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(string text, int rotation, int positionX, int positionY, int scale)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.GraphicTicketBarcodeQR(
                    _deviceConfig.OperatorPassword,
                    Print: true,
                    SaveOnFile: false,
                    Rotation: rotation,
                    positionX: positionX,
                    positionY: positionY,
                    scale: scale, // 1 - 8
                    text: text);

                return CheckRespose(deviceResponse);
            }
            catch (Exception exception)
            {
                AddException(exception);
                return false;
            }
        }
    }
}
