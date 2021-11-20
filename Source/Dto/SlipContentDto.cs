using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class SlipContentDto
    {
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
    }
}
