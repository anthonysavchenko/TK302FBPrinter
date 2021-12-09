namespace TK302FBPrinter.Configuration
{
    public class TextLine
    {
        public string Text { get; set; } = string.Empty;

        public int Rotation { get; set; } = 2;

        public int PositionX { get; set; } = 1;

        public int PositionY { get; set; } = 1;

        public int FontSize { get; set; } = 3;

        public int ScaleX { get; set; } = 1;
        
        public int ScaleY { get; set; } = 1;

        public int FontStyle { get; set; } = 11;
    }

    public class Template
    {
        public string TemplateName { get; set; } = string.Empty;

        public TextLine[] TextLines { get; set; } = {};
    }

    public class TicketConfig
    {
        public const string SectionName = "Ticket";

        public Template[] Templates { get; set; } = {};
    }
}
