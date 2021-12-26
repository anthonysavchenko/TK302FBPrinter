using System.ComponentModel;

namespace TK302FBPrinter.Dto
{
    public class ShiftCloseDto
    {
        [DefaultValue(true)]
        public bool Cut { get; set; }
    }
}
