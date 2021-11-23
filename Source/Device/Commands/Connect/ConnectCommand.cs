using System;
using System.IO.Ports;
using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.Connect
{
    public class ConnectCommand : DeviceCommand, IConnectCommand
    {
        public ConnectCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig.Value) {}

        public bool Execute()
        {
            if (_deviceConnector.Connection != null)
            {
                return true;
            }

            _deviceConnector.Connection = new ProtocolAPI()
            {
                ComunicationType = ComunicationTypeEnum.RS232,
                ComunicationParams = new object[]
                {
                    57600, // BaudRate
                    8, // DataBits
                    Handshake.None, // Handshake
                    Parity.None, // Parity
                    _deviceConfig.PortName, // PortName
                    StopBits.One, // StopBits
                    false, // Dtr
                    false // Rts
                }
            };

            try
            {
                var deviceResponse = _deviceConnector.Connection.OpenConnection();
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
