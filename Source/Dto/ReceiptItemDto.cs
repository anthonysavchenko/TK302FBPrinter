using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class ReceiptItemDto
    {
        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [Range(1, 1e3)]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 1e7)]
        public int Price { get; set; }
    }    
}
