using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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

    public class SeatDto
    {
        [Required]
        [Range(1, 100)]
        public int Row { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Place { get; set; }
    }

    public class TicketDto
    {
        [Required]
        [MaxLength(250)]
        public string TemplateName { get; set; }

        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Cut { get; set; }

        public PlaceholderDto[] Placeholders { get; set; } = {};

        public SeatDto[] Seats { get; set; } = {};
    }
}
