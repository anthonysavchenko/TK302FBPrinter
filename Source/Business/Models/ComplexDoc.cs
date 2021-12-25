namespace TK302FBPrinter.Business.Models
{
    // Слип-чек в составном документе
    public class ComplexDocSlip
    {
        // Текст (строка с разделителями, по которым она будет разбита на несколько строк)
        public string Text { get; set; }

        // Признак отреза бумаги после печати
        public bool Cut { get; set; }
    }

    // Составной документ
    public class ComplexDoc
    {
        // Кинобилеты
        public Ticket Ticket { get; set; }

        // Слип-чек с информацией банка об оплате
        public ComplexDocSlip Slip { get; set; }

        // Кассовый чек для кинобилетов (кроме товаров)
        public Receipt TicketReceipt { get; set; }

        // Кассовый чек для товаров (кроме кинобилетов)
        public Receipt GoodsReceipt { get; set; }
    }
}
