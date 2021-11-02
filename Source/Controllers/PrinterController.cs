using System.IO.Ports;
using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.Comunication;
using Custom.Fiscal.RUSProtocolAPI.CustomRU;
using Microsoft.AspNetCore.Mvc;

namespace TK302FBPrinter
{
    [Route("[controller]")]
    [ApiController]
    public class PrinterController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetStatus()
        {
            Connect();
            Beep();
            Disconnect();
            return NoContent();
        }
        
        private static ProtocolAPI printerAPI = new ProtocolAPI();
        private SerialPortParams rs232Params = null;
        private string OperatorPassword = "999999";


        private void Connect()
        {
            printerAPI.ComunicationType = Custom.Fiscal.RUSProtocolAPI.Enums.ComunicationTypeEnum.RS232;

            rs232Params = new SerialPortParams()
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

            printerAPI.ComunicationParams = new object[]{ rs232Params.BaudRate,
                                                              rs232Params.DataBits,
                                                              rs232Params.HandshakeProp,
                                                              rs232Params.Parity,
                                                              rs232Params.PortName,
                                                              rs232Params.StopBits,
                                                              rs232Params.Dtr,
                                                              rs232Params.Rts };

            var cmdResponse = printerAPI.OpenConnection();
        }

        private void Beep()
        {
            APIBaseResponse cmdResponse = printerAPI.Beep(OperatorPassword);
        }

        private void Disconnect()
        {
            var cmdResponse = printerAPI.CloseConnection();
        }
    }
}
