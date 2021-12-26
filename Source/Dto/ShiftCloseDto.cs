using System.ComponentModel;
using Newtonsoft.Json;

namespace TK302FBPrinter.Dto
{
    public class ShiftCloseDto
    {
        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Cut { get; set; }
    }
}
