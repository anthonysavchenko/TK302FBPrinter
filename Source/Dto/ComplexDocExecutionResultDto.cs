using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{

    public enum ComplexDocStatus
    {
        Success,
        Error
    }

    public class ComplexDocExecutionResultDto
    {
        public ComplexDocExecutionResultDto(string errorDescription = null)
        {
            if (!string.IsNullOrEmpty(errorDescription))
            {
                ErrorDescription = errorDescription;
                Status = ComplexDocStatus.Error;
                return;
            }
            ErrorDescription = string.Empty;
            Status = ComplexDocStatus.Success;
        }

        [Required]
        [EnumDataType(typeof(ComplexDocStatus))]
        public ComplexDocStatus Status { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ErrorDescription { get; set; }
    }
}
