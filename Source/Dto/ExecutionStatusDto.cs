using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TK302FBPrinter.Dto
{

    public enum ExecutionStatus
    {
        [EnumMember(Value = "success")]
        Success,

        [EnumMember(Value = "error")]
        Error
    }

    public class ExecutionStatusDto
    {
        public ExecutionStatusDto(string errorDescription = null)
        {
            if (!string.IsNullOrEmpty(errorDescription))
            {
                Error = errorDescription;
                Description = errorDescription;
                Status = ExecutionStatus.Error;
                return;
            }
            Error = string.Empty;
            Description = string.Empty;
            Status = ExecutionStatus.Success;
        }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExecutionStatus Status { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Error { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
