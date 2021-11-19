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
            ProtocolAPI connection,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connection)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            if (_connection != null)
            {
                return true;
            }

            _connection = new ProtocolAPI()
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

            var deviceResponse = _connection.OpenConnection();
            return CheckRespose(deviceResponse);
        }
    }
}
