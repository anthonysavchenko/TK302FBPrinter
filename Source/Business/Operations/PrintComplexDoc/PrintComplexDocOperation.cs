using Microsoft.Extensions.Options;
using TK302FBPrinter.Configuration;
using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Business.Models;
using TK302FBPrinter.Business.Operations.PrintReceipt;
using TK302FBPrinter.Business.Operations.PrintTicket;
using TK302FBPrinter.Device.Commands.GraphicDocOpen;
using TK302FBPrinter.Device.Commands.GraphicDocClose;
using TK302FBPrinter.Device.Commands.GraphicDocTextAdd;
using TK302FBPrinter.Device.Commands.Cut;

namespace TK302FBPrinter.Business.Operations.PrintComplexDoc
{
    public class PrintComplexDocOperation : Operation, IPrintComplexDocOperation
    {
        private readonly SlipConfig _slipConfig;
        private readonly ComplexDocConfig _complexDocConfig;
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IGraphicDocOpenCommand _graphicDocOpenCommand;
        private readonly IGraphicDocCloseCommand _graphicDocCloseCommand;
        private readonly IGraphicDocTextAddCommand _graphicDocTextAddCommand;
        private readonly IPrintTicketOperation _printTicketOperation;
        private readonly IPrintReceiptOperation _printReceiptOperation;
        private readonly ICutCommand _cutCommand;

        public PrintComplexDocOperation(
            IOptions<SlipConfig> slipConfig,
            IOptions<ComplexDocConfig> complexDocConfig,
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IGraphicDocOpenCommand graphicDocOpenCommand,
            IGraphicDocCloseCommand graphicDocCloseCommand,
            IGraphicDocTextAddCommand graphicDocTextAddCommand,
            IPrintTicketOperation printTicketOperation,
            IPrintReceiptOperation printReceiptOperation,
            ICutCommand cutCommand)
        {
            _slipConfig = slipConfig.Value;
            _complexDocConfig = complexDocConfig.Value;
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _graphicDocOpenCommand = graphicDocOpenCommand;
            _graphicDocCloseCommand = graphicDocCloseCommand;
            _graphicDocTextAddCommand = graphicDocTextAddCommand;
            _printTicketOperation = printTicketOperation;
            _printReceiptOperation = printReceiptOperation;
            _cutCommand = cutCommand;
        }

        public bool Execute(ComplexDoc complexDoc)
        {
            if (!_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (complexDoc.Ticket != null && !_printTicketOperation.Execute(
                complexDoc.Ticket,
                _complexDocConfig.PrintTicketOnly))
            {
                AddErrorDescription(_printTicketOperation.ErrorDescriptions);
                Disconnect();
                return false;
            }

            if (complexDoc.Slip != null && !PrintSlip(complexDoc.Slip, !_complexDocConfig.PrintTicketOnly))
            {
                Disconnect();
                return false;
            }

            if (complexDoc.TicketReceipt != null && !_printReceiptOperation.Execute(
                complexDoc.TicketReceipt,
                !_complexDocConfig.PrintTicketOnly))
            {
                AddErrorDescription(_printReceiptOperation.ErrorDescriptions);
                Disconnect();
                return false;
            }

            if (complexDoc.GoodsReceipt != null && !_printReceiptOperation.Execute(
                complexDoc.GoodsReceipt,
                !_complexDocConfig.PrintTicketOnly))
            {
                AddErrorDescription(_printReceiptOperation.ErrorDescriptions);
                Disconnect();
                return false;
            }

            Disconnect();
            return true;
        }

        private bool PrintSlip(ComplexDocSlip slip, bool print)
        {
            if (!print)
            {
                return true;
            }

            var slipLines = slip.Text.Split(_slipConfig.LineSeparators, System.StringSplitOptions.None);
            var sizeX = 576;
            var sizeY = (slipLines.Length + 1) * 25;

            if (!_graphicDocOpenCommand.Execute(sizeX, sizeY))
            {
                AddErrorDescription(_graphicDocOpenCommand.ErrorDescription);
                return false;
            }

            if (!PrintSlipLines(slipLines))
            {
                return false;
            }

            if (!_graphicDocCloseCommand.Execute())
            {
                AddErrorDescription(_graphicDocCloseCommand.ErrorDescription);
                return false;
            }

            if (slip.Cut && !_cutCommand.Execute())
            {
                AddErrorDescription(_cutCommand.ErrorDescription);
                return false;
            }            

            return true;
        }

        private bool PrintSlipLines(string[] slipLines)
        {
            for (var index = 0; index < slipLines.Length; index++)
            {
                if (!string.IsNullOrEmpty(slipLines[index])
                    && !_graphicDocTextAddCommand.Execute(
                        slipLines[index],
                        rotation: 1,
                        positionX: 0,
                        positionY: index * 25,
                        fontSize: 1,
                        scaleX: 1,
                        scaleY: 2,
                        fontStyle: 11))
                {
                    AddErrorDescription(_graphicDocTextAddCommand.ErrorDescription);
                    return false;
                }
            }

            return true;
        }

        private void Disconnect()
        {
            if (!_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
            }
        }
    }
}
