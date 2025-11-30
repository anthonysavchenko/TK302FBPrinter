namespace TK302FBPrinter.Business.Models
{
    public enum PaymentType {
        Card,
        Bonus,
        PushkinCard,
        GiftCard
    }

    public class Placeholder
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public class Seat
    {
        public int Row { get; set; }

        public int Place { get; set; }
    }

    public class Ticket
    {
        public string TemplateName { get; set; }

        public PaymentType PaymentType { get; set; }

        public string Hall { get; set; }

        public string Format { get; set; }

        public Placeholder[] Placeholders { get; set; }

        public Seat[] Seats { get; set; }

        public bool WithConnection { get; set; }

        public bool Cut { get; set; }
    }
}
