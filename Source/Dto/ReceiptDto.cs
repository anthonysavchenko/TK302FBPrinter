using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
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
