using System.IO.Ports;
using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.Comunication;
using Custom.Fiscal.RUSProtocolAPI.CustomRU;
using Custom.Fiscal.RUSProtocolAPI.Enums;

namespace TK302FBPrinter.Printer
{
    public class PrinterConnector : IPrinterConnector
    {
        private ProtocolAPI _connection;
        private string _errorDescription;

        private bool CheckRespose(APIBaseResponse response)
        {
            if (response.ErrorCode != 0)
            {
                _errorDescription = $"{response.ErrorDescription}. Код оператора: {response.OperatorCode}";
                return false;
            }
            return true;            
        }

        private bool Connect()
        {
            if (_connection != null)
            {
                return true;
            }

            var serialPortParams = new SerialPortParams()
            {
                BaudRate = 57600,
                DataBits = 8,
                HandshakeProp = Handshake.None,
                Parity = Parity.None,
                PortName = "COM3",
                StopBits = StopBits.One,
                Dtr = false,
                Rts = false
            };

            _connection = new ProtocolAPI()
            {
                ComunicationType = ComunicationTypeEnum.RS232,
                ComunicationParams = new object[]
                {
                    serialPortParams.BaudRate,
                    serialPortParams.DataBits,
                    serialPortParams.HandshakeProp,
                    serialPortParams.Parity,
                    serialPortParams.PortName,
                    serialPortParams.StopBits,
                    serialPortParams.Dtr,
                    serialPortParams.Rts
                }
            };

            var printerResponse = _connection.OpenConnection();

            return CheckRespose(printerResponse);
        }

        private bool Disconnect()
        {
            if (_connection == null)
            {
                return true;
            }
            var printerResponse = _connection.CloseConnection();
            return CheckRespose(printerResponse);
        }

        public bool Beep()
        {
            var operatorPassword = "999999";

            if (!Connect())
            {
                return false;
            }

            var printerResponse = _connection.Beep(operatorPassword);
            if (!CheckRespose(printerResponse))
            {
                return false;
            }
            

            if (!Disconnect())
            {
                return false;
            }

            return true;
        }
    }
}
