using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class SlipContentDto
    {
        [Required]
        [MaxLength(250)]
        public string Text { get; set; }
    }
}
