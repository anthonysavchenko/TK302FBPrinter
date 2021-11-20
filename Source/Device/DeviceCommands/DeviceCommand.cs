using System;
using Custom.Fiscal.RUSProtocolAPI.CustomRU;

namespace TK302FBPrinter.Device.DeviceCommands
{
    public class DeviceCommand
    {
        protected Connector _connector;

        public string ErrorDescription { get; private set; }
        
        public DeviceCommand(Connector connector)
        {
            _connector = connector;
        }

        protected bool CheckRespose(APIBaseResponse response)
        {
            if (response.ErrorCode != 0)
            {
                ErrorDescription =
                    "DeviceError. "
                        + $"ErrorCode: {response.ErrorCode}. "
                        + $"ErrorDescription: {response.ErrorDescription}. "
                        + $"OperatorCode: {response.OperatorCode}";
                return false;
            }
            return true;            
        }

        protected void AddException(Exception exception)
        {
            ErrorDescription =
                "WebServiceError. "
                    + "ErrorDescription: Ошибка во время отправки соманды на ККТ, возможно, соединение с ККТ потеряно "
                    + "или закрыто. "
                    + $"ExceptionMessage: {exception.Message}";
        }
    }
}
