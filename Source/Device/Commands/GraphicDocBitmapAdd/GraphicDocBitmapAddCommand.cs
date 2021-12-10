using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.GraphicDocBitmapAdd
{
    public class GraphicDocBitmapAddCommand : DeviceCommand, IGraphicDocBitmapAddCommand
    {
        public GraphicDocBitmapAddCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(int bitmapId, int rotation, int positionX, int positionY, int scaleX, int scaleY)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.InsertBitmapToGraphicTicket(
                    _deviceConfig.OperatorPassword,
                    Print: true,
                    SaveOnFile: false,
                    Rotatoin: rotation,
                    BitmapX: positionX,
                    BitmapY: positionY,
                    BitmapIndex: bitmapId,
                    ScaleX: scaleX,
                    ScaleY: scaleY);

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
