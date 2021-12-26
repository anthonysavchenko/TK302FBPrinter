using System.ComponentModel;

namespace TK302FBPrinter.Dto
{
    public class ReportXDto
    {
        [DefaultValue(true)]
        public bool Cut { get; set; }
    }
}
