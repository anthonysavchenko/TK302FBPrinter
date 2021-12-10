using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class TicketCinemaDto
    {
        [Required]
        [MaxLength(250)]
        public string TemplateName { get; set; }

        public string Theatre { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string OGRN { get; set; }

        public string INN { get; set; }

        public string Movie { get; set; }

        public string Format { get; set; }
    }
}
