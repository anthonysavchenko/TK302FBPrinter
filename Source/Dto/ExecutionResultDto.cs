using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class ExecutionResultDto
    {
        public ExecutionResultDto()
        {
            Succeed = true;
            ErrorDescription = string.Empty;
        }

        public ExecutionResultDto(string errorDescription)
        {
            Succeed = false;
            ErrorDescription = errorDescription;
        }

        [Required]
        public bool Succeed { get; set; }

        [Required]
        [MaxLength(250)]
        public string ErrorDescription { get; set; }
    }
}
