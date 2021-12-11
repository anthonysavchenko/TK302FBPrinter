using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class PlaceholderDto
    {
        [Required]
        [MaxLength(250)]
        public string Key { get; set; }

        [Required]
        [MaxLength(250)]
        public string Value { get; set; }
    }

    public class TicketDto
    {
        [Required]
        [MaxLength(250)]
        public string TemplateName { get; set; }

        public PlaceholderDto[] Placeholders { get; set; } = {};
    }
}
