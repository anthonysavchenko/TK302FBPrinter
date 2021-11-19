using Custom.Fiscal.RUSProtocolAPI;
using Custom.Fiscal.RUSProtocolAPI.CustomRU;

namespace TK302FBPrinter.Device.DeviceCommands
{
    public class DeviceCommand
    {
        protected ProtocolAPI _connection;

        public string ErrorDescription { get; private set; }
        
        public DeviceCommand(ProtocolAPI connection)
        {
            _connection = connection;
        }

        protected bool CheckRespose(APIBaseResponse response)
        {
            if (response.ErrorCode != 0)
            {
                ErrorDescription =
                    $"DeviceError. ErrorCode: {response.ErrorCode}. "
                        + $"ErrorDescription: {response.ErrorDescription}. "
                        + $"OperatorCode: {response.OperatorCode}";
                return false;
            }
            return true;            
        }
    }
}
