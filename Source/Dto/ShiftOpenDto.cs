using System.ComponentModel;

namespace TK302FBPrinter.Dto
{
    public class ShiftOpenDto
    {
        [DefaultValue(true)]
        public bool Cut { get; set; }
    }
}
