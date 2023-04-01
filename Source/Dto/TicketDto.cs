using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TK302FBPrinter.Dto
{
    public enum PaymentTypeDto {

        Card,
        Bonus,
        PushkinCard
    }

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

        [EnumDataType(typeof(PaymentTypeDto))]
        public PaymentTypeDto PaymentType { get; set; }

        public PlaceholderDto[] Placeholders { get; set; } = {};

        public SeatDto[] Seats { get; set; } = {};

        [MaxLength(250)]
        public string Hall { get; set; }

        [MaxLength(250)]
        public string Format { get; set; }
    }
}
