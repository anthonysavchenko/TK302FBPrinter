using TK302FBPrinter.Device.Commands.Connect;
using TK302FBPrinter.Device.Commands.Disconnect;
using TK302FBPrinter.Device.Commands.ReceiptItemAdd;
using TK302FBPrinter.Device.Commands.ReceiptCancel;
using TK302FBPrinter.Device.Commands.ReceiptClose;
using TK302FBPrinter.Device.Commands.ReceiptOpen;
using TK302FBPrinter.Business.Models;
using TK302FBPrinter.Device.Commands.Cut;
using TK302FBPrinter.Device.Commands.ReceiptItemAgentFlagSet;
using TK302FBPrinter.Device.Commands.ReceiptItemSupplierNameSet;
using TK302FBPrinter.Device.Commands.ReceiptItemSupplierPhoneSet;
using TK302FBPrinter.Device.Commands.ReceiptItemSupplierINNSet;

namespace TK302FBPrinter.Business.Operations.PrintReceipt
{
    public class PrintReceiptOperation : Operation, IPrintReceiptOperation
    {
        private readonly IConnectCommand _connectCommand;
        private readonly IDisconnectCommand _disconnectCommand;
        private readonly IReceiptOpenCommand _receiptOpenCommand;
        private readonly IReceiptCloseCommand _receiptCloseCommand;
        private readonly IReceiptItemAddCommand _receiptItemAddCommand;
        private readonly IReceiptCancelCommand _receiptCancelCommand;
        private readonly IReceiptItemAgentFlagSetCommand _receiptItemAgentFlagSetCommand;
        private readonly IReceiptItemSupplierNameSetCommand _receiptItemSupplierNameSetCommand;
        private readonly IReceiptItemSupplierINNSetCommand _receiptItemSupplierINNSetCommand;
        private readonly IReceiptItemSupplierPhoneSetCommand _receiptItemSupplierPhoneSetCommand;
        private readonly ICutCommand _cutCommand;

        public PrintReceiptOperation(
            IConnectCommand connectCommand,
            IDisconnectCommand disconnectCommand,
            IReceiptOpenCommand receiptOpenCommand,
            IReceiptCloseCommand receiptCloseCommand,
            IReceiptItemAddCommand receiptItemAddCommand,
            IReceiptCancelCommand receiptCancelCommand,
            IReceiptItemAgentFlagSetCommand receiptItemAgentFlagSetCommand,
            IReceiptItemSupplierNameSetCommand receiptItemSupplierNameSetCommand,
            IReceiptItemSupplierINNSetCommand receiptItemSupplierINNSetCommand,
            IReceiptItemSupplierPhoneSetCommand receiptItemSupplierPhoneSetCommand,
            ICutCommand cutCommand)
        {
            _connectCommand = connectCommand;
            _disconnectCommand = disconnectCommand;
            _receiptOpenCommand = receiptOpenCommand;
            _receiptCloseCommand = receiptCloseCommand;
            _receiptItemAddCommand = receiptItemAddCommand;
            _receiptCancelCommand = receiptCancelCommand;
            _receiptItemAgentFlagSetCommand = receiptItemAgentFlagSetCommand;
            _receiptItemSupplierNameSetCommand = receiptItemSupplierNameSetCommand;
            _receiptItemSupplierINNSetCommand = receiptItemSupplierINNSetCommand;
            _receiptItemSupplierPhoneSetCommand = receiptItemSupplierPhoneSetCommand;
            _cutCommand = cutCommand;            
        }

        public bool Execute(Receipt receipt)
        {
            if (receipt.WithConnection && !_connectCommand.Execute())
            {
                AddErrorDescription(_connectCommand.ErrorDescription);
                return false;
            }

            if (!_receiptOpenCommand.Execute(receipt.IsReturn, (int)receipt.Tax))
            {
                AddErrorDescription(_receiptOpenCommand.ErrorDescription);
                Disconnect(receipt.WithConnection);
                return false;
            }

            foreach (var item in receipt.Items)
            {
                if (receipt.Supplier != null)
                {
                    if (!_receiptItemAgentFlagSetCommand.Execute())
                    {
                        AddErrorDescription(_receiptItemAgentFlagSetCommand.ErrorDescription);
                        CancelReceipt();
                        Disconnect(receipt.WithConnection);
                        return false;
                    }

                    if (!_receiptItemSupplierNameSetCommand.Execute(receipt.Supplier.CompanyName))
                    {
                        AddErrorDescription(_receiptItemSupplierNameSetCommand.ErrorDescription);
                        CancelReceipt();
                        Disconnect(receipt.WithConnection);
                        return false;
                    }

                    if (!_receiptItemSupplierINNSetCommand.Execute(receipt.Supplier.INN))
                    {
                        AddErrorDescription(_receiptItemSupplierINNSetCommand.ErrorDescription);
                        CancelReceipt();
                        Disconnect(receipt.WithConnection);
                        return false;
                    }

                    if (!_receiptItemSupplierPhoneSetCommand.Execute(receipt.Supplier.Phone))
                    {
                        AddErrorDescription(_receiptItemSupplierPhoneSetCommand.ErrorDescription);
                        CancelReceipt();
                        Disconnect(receipt.WithConnection);
                        return false;
                    }
                }

                if (!_receiptItemAddCommand.Execute(
                    item.Description,
                    item.Quantity,
                    item.Price,
                    (int)item.VAT,
                    receipt.IsReturn))
                {
                    AddErrorDescription(_receiptItemAddCommand.ErrorDescription);
                    CancelReceipt();
                    Disconnect(receipt.WithConnection);
                    return false;
                }
            }

            if (!_receiptCloseCommand.Execute(receipt.Total))
            {
                AddErrorDescription(_receiptCloseCommand.ErrorDescription);
                CancelReceipt();
                Disconnect(receipt.WithConnection);
                return false;
            }

            if (receipt.Cut && !_cutCommand.Execute())
            {
                AddErrorDescription(_cutCommand.ErrorDescription);
                Disconnect(receipt.WithConnection);
                return false;
            }

            Disconnect(receipt.WithConnection);
            return true;
        }

        private void CancelReceipt()
        {
            if (!_receiptCancelCommand.Execute())
            {
                AddErrorDescription(_receiptCancelCommand.ErrorDescription);
            }
        }

        private void Disconnect(bool withConnection)
        {
            if (withConnection && !_disconnectCommand.Execute())
            {
                AddErrorDescription(_disconnectCommand.ErrorDescription);
            }
        }
    }
}
