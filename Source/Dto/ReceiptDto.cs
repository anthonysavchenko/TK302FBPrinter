using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public enum TaxTypeDto
    {
        AutomaticMode,
        Traditional,
        LightIncome,
        LightIncomeNoExpenses,
        SingleTax,
        Agricultural,
        Patent
    }

    public enum VATTypeDto
    {
        NoVAT,
        Percent0,
        Percent10,
        Percent20,
        Percent10Base110,
        Percent20Base120,
    }
        
    public class ReceiptItemDto
    {
        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [Range(1, 1e5)]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 1e7)]
        public int Price { get; set; }

        [Required]
        [EnumDataType(typeof(VATTypeDto))]
        public VATTypeDto VAT { get; set; }
    }

    public class SupplierDto
    {
        [Required]
        [MaxLength(250)]
        public string INN { get; set; }

        [Required]
        [MaxLength(250)]
        public string CompanyName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Phone { get; set; }        
    }

    public class ReceiptDto
    {
        [Required]
        public ReceiptItemDto[] Items { get; set; }

        [Required]
        [EnumDataType(typeof(TaxTypeDto))]
        public TaxTypeDto Tax { get; set; }
        
        public SupplierDto Supplier { get; set; }

        public bool IsReturn { get; set; }

        [Required]
        [Range(1, 1e7)]
        public int Total { get; set; }
    }
}
