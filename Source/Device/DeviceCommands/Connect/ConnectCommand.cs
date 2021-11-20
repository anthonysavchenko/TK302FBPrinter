using System;
using System.IO.Ports;
using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.Enums;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.Connect
{
    public class ConnectCommand : DeviceCommand, IConnectCommand
    {
        private readonly PrinterOptions _printerOptions;

        public ConnectCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            if (_connector.Connection != null)
            {
                return true;
            }

            _connector.Connection = new ProtocolAPI()
            {
                ComunicationType = ComunicationTypeEnum.RS232,
                ComunicationParams = new object[]
                {
                    57600, // BaudRate
                    8, // DataBits
                    Handshake.None, // Handshake
                    Parity.None, // Parity
                    _printerOptions.PortName, // PortName
                    StopBits.One, // StopBits
                    false, // Dtr
                    false // Rts
                }
            };

            try
            {
                var deviceResponse = _connector.Connection.OpenConnection();
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
