namespace TK302FBPrinter.Business.Models
{
    public enum TaxType
    {
        AutomaticMode = 1,
        Traditional,
        LightIncome,
        LightIncomeNoExpenses,
        SingleTax,
        Agricultural,
        Patent
    }

    public enum VATType
    {
        NoVAT = 1,
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

    public class Receipt
    {
        public ReceiptItem[] Items { get; set; }

        public TaxType Tax { get; set; }
        
        public bool IsReturn { get; set; }

        public int Total { get; set; }

        public bool WithConnection { get; set; }
    }
}