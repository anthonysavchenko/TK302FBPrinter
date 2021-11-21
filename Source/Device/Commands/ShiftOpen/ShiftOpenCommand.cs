using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.Commands.ShiftOpen
{
    public class ShiftOpenCommand : DeviceCommand, IShiftOpenCommand
    {
        public ShiftOpenCommand(
            DeviceConnector deviceConnector,
            IOptionsSnapshot<DeviceConfig> deviceConfig) : base(deviceConnector, deviceConfig) {}

        public bool Execute()
        {
            bool print = true; // Печатаь фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)
            
            try
            {
                var deviceResponse = _deviceConnector.Connection.OpenFiscalDay(
                    _deviceConfig.OperatorPassword,
                    print,
                    saveOnFile);

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
