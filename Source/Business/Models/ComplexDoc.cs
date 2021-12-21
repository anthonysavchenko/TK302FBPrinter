namespace TK302FBPrinter.Business.Models
{
    public class ComplexDocSlip
    {
        public string Text { get; set; }

        public bool Cut { get; set; }
    }

    public class ComplexDoc
    {
        public ComplexDocSlip Slip { get; set; }

        public Ticket Ticket { get; set; }

        public Receipt Receipt { get; set; }
    }
}
