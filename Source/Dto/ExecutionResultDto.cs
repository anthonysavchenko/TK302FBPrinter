using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class ExecutionResultDto
    {
        public ExecutionResultDto(string errorDescription = null)
        {
            if (!string.IsNullOrEmpty(errorDescription))
            {
                ErrorDescription = errorDescription;
                Succeed = false;
                return;
            }
            ErrorDescription = string.Empty;
            Succeed = true;
        }

        [Required]
        public bool Succeed { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ErrorDescription { get; set; }
    }
}
