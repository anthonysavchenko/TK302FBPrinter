namespace TK302FBPrinter.Business.Models
{
    public class Placeholder
    {
        public string Key { get; set; }

        public string Replacement { get; set; }
    }

    public class Ticket
    {
        public string TemplateName { get; set; }

        public Placeholder[] Placeholders { get; set; }
    }
}