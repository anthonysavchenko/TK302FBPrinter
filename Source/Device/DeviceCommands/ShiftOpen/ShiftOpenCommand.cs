using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;

namespace TK302FBPrinter.Device.DeviceCommands.ShiftOpen
{
    public class ShiftOpenCommand : DeviceCommand, IShiftOpenCommand
    {
        private readonly PrinterOptions _printerOptions;

        public ShiftOpenCommand(
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
                var deviceResponse = _connector.Connection.OpenFiscalDay(
                    _printerOptions.OperatorPassword,
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
