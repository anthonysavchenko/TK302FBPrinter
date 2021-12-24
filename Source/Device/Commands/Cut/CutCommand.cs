using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.Cut
{
    public class CutCommand : DeviceCommand, ICutCommand
    {
        public CutCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute()
        {
            try
            {
                var deviceResponse = _deviceConnector.Connection.PaperCut(_deviceConfig.OperatorPassword);

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
