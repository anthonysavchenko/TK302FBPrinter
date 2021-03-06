using System;
using Custom.Fiscal.RUSProtocolAPI.CustomRU;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands
{
    public class DeviceCommand
    {
        protected readonly DeviceConnector _deviceConnector;
        protected readonly DeviceConfig _deviceConfig;

        public string ErrorDescription { get; private set; }
        
        public DeviceCommand(DeviceConnector deviceConnector, DeviceConfig deviceConfig)
        {
            _deviceConnector = deviceConnector;
            _deviceConfig = deviceConfig;
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
