using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.GraphicDocClose
{
    public class GraphicDocCloseCommand : DeviceCommand, IGraphicDocCloseCommand
    {
        public GraphicDocCloseCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute(bool cut)
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.GraphicTicketClose(
                    _deviceConfig.OperatorPassword,
                    Print: true,
                    saveOnFile: false,
                    cutPaper: cut);

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
