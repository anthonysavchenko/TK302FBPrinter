namespace TK302FBPrinter.Business.Models
{
    public enum TaxType
    {
        AutomaticMode,
        Traditional,
        LightIncome,
        LightIncomeNoExpenses,
        SingleTax,
        Agricultural,
        Patent
    }

    public enum VATType
    {
        NoVAT,
        Percent0,
        Percent10,
        Percent20,
        Percent10Base110,
        Percent20Base120,
    }
        
    public class ReceiptItem
    {
        public string Description { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public VATType VAT { get; set; }
    }    

    public class Supplier
    {
        public string INN { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }        
    }

    public class Receipt
    {
        public ReceiptItem[] Items { get; set; }

        public TaxType Tax { get; set; }

        public Supplier Supplier { get; set; }
        
        public bool IsReturn { get; set; }

        public int Total { get; set; }

        public bool WithConnection { get; set; }

        public bool Cut { get; set; }
    }
}