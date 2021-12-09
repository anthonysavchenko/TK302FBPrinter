using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class Placeholder
    {
        [Required]
        [MaxLength(250)]
        public string Key { get; set; }

        [Required]
        [MaxLength(250)]
        public string Replacement { get; set; }
    }

    public class TicketDto
    {
        [Required]
        [MaxLength(250)]
        public string TemplateName { get; set; }

        public Placeholder[] Placeholders { get; set; } = {};
    }
}
