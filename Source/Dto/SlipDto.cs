using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TK302FBPrinter.Dto
{
    public class SlipDto
    {
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }

        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Cut { get; set; }
    }
}
