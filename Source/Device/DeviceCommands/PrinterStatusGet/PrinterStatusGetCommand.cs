using System;
using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Dto;

namespace TK302FBPrinter.Device.DeviceCommands.PrinterStatusGet
{
    public class PrinterStatusGetCommand : DeviceCommand, IPrinterStatusGetCommand
    {
        private readonly PrinterOptions _printerOptions;

        public PrinterStatusGetCommand(
            Connector connector,
            IOptionsSnapshot<PrinterOptions> printerOptions) : base(connector)
        {
            _printerOptions = printerOptions.Value;
        }

        public bool Execute(out PrinterStatusDto status)
        {
            status = null;

            try
            {
                var deviceResponse = _connector.Connection.GetPrinterStatus(_printerOptions.OperatorPassword);

                if (!CheckRespose(deviceResponse))
                {
                    return false;
                }

                status = new PrinterStatusDto()
                {
                    CoverOpen = deviceResponse.CoverOpen,
                    CutterError = deviceResponse.CutterError,
                    DateNotSet = deviceResponse.DateNotSet,
                    FWUpadteWaiting = deviceResponse.FWUpadteWaiting,
                    HWInitJumperOn = deviceResponse.HWInitJumperOn,
                    PaperJam = deviceResponse.PaperJam,
                    PaperNearEnd = deviceResponse.PaperNearEnd,
                    PaperPresent = deviceResponse.PaperPresent,
                    PrinterError = deviceResponse.PrinterError,
                    PrinterIdle = deviceResponse.PrinterIdle,
                    Printing = deviceResponse.Printing,
                    ResetNeeded = deviceResponse.ResetNeeded,
                    Serialized = deviceResponse.Serialized,
                    ShiftOpen = deviceResponse.DayOpen,
                    TicketOut = deviceResponse.TicketOut,
                    VirtualPaperNearEnd = deviceResponse.VirtualPaperNearEnd
                };
                return true;
            }
            catch (Exception exception)
            {
                AddException(exception);
                return false;
            }
        }
    }
}
