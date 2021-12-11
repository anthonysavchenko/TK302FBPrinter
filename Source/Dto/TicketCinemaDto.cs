using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class TicketCinemaDto
    {
        [Required]
        [MaxLength(250)]
        public string TemplateName { get; set; }

        [MaxLength(250)]
        public string Theatre { get; set; }

        [MaxLength(250)]
        public string CompanyName { get; set; }

        [MaxLength(250)]
        public string CompanyAddress { get; set; }

        [MaxLength(250)]
        public string OGRN { get; set; }

        [MaxLength(250)]
        public string INN { get; set; }

        [MaxLength(250)]
        public string Movie { get; set; }

        [MaxLength(250)]
        public string Format { get; set; }
    }
}
