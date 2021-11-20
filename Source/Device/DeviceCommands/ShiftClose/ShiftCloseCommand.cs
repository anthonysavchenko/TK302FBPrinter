using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.ShiftClose
{
    public class ShiftCloseCommand : DeviceCommand, IShiftCloseCommand
    {
        private readonly PrinterOptions _printerOptions;

        public ShiftCloseCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute()
        {
            bool print = true; // Печатаь фискальный документ (ФД)
            bool saveOnFile = false; // Не сохранять ФД в памяти ККТ (формат документа .spl)
            
            try
            {
                var deviceResponse = _connector.Connection.ZReport(_printerOptions.OperatorPassword, print, saveOnFile);
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
