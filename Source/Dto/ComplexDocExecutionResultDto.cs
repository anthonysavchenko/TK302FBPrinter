using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TK302FBPrinter.Dto
{

    public enum ComplexDocStatus
    {
        [EnumMember(Value = "success")]
        Success,

        [EnumMember(Value = "error")]
        Error
    }

    public class ComplexDocExecutionResultDto
    {
        public ComplexDocExecutionResultDto(string errorDescription = null)
        {
            if (!string.IsNullOrEmpty(errorDescription))
            {
                Error = errorDescription;
                Description = errorDescription;
                Status = ComplexDocStatus.Error;
                return;
            }
            Error = string.Empty;
            Description = string.Empty;
            Status = ComplexDocStatus.Success;
        }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ComplexDocStatus Status { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Error { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
