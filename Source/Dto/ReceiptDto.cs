using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
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
        [EnumDataType(typeof(VATType))]
        public VATType VAT { get; set; }
    }    

    public class ReceiptDto
    {
        [Required]
        public List<ReceiptItemDto> Items { get; set; }

        [Required]
        [EnumDataType(typeof(TaxType))]
        public TaxType Tax { get; set; }
        
        public bool IsReturn { get; set; }

        [Required]
        [Range(1, 1e7)]
        public int Total { get; set; }
    }
}
