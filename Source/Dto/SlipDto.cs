using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class SlipDto
    {
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }

        [DefaultValue(true)]
        public bool Cut { get; set; }
    }
}
