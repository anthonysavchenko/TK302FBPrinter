using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class SlipDto
    {
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }
    }
}
