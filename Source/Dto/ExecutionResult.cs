using System.ComponentModel.DataAnnotations;

namespace TK302FBPrinter.Dto
{
    public class ExecutionResult
    {
        public ExecutionResult()
        {
            Succeed = true;
            ErrorDescription = string.Empty;
        }

        public ExecutionResult(string errorDescription)
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
