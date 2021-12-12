namespace TK302FBPrinter.Configuration
{
    public class Line
    {
        public int PositionX1 { get; set; } = 1;

        public int PositionY1 { get; set; } = 1;

        public int PositionX2 { get; set; } = 1;

        public int PositionY2 { get; set; } = 1;

        public int Width { get; set; } = 1;
    }
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

    public class QrCode
    {
        public string Text { get; set; } = string.Empty;

        public int Rotation { get; set; } = 2;

        public int PositionX { get; set; } = 1;

        public int PositionY { get; set; } = 1;

        public int Scale { get; set; } = 1;
    }

    public class Bitmap
    {
        public int BitmapId { get; set; } = 1;

        public int Rotation { get; set; } = 2;

        public int PositionX { get; set; } = 1;

        public int PositionY { get; set; } = 1;

        public int ScaleX { get; set; } = 1;
        
        public int ScaleY { get; set; } = 1;
    }

    public class Template
    {
        public string TemplateName { get; set; } = string.Empty;
        
        public int SizeX { get; set; } = 1;

        public int SizeY { get; set; } = 1;

        public Line[] Lines { get; set; } = {};

        public TextLine[] TextLines { get; set; } = {};
        
        public TextLine[] SeatTextLines { get; set; } = {};

        public QrCode[] QrCodes { get; set; } = {};

        public Bitmap[] Bitmaps { get; set; } = {};
    }

    public class TicketConfig
    {
        public const string SectionName = "Ticket";

        public string SeatsPlaceholder { get; set; }

        public string SeatsSeparator { get; set; }

        public string SeatsRowPlaceholder { get; set; }

        public string SeatsPlacePlaceholder { get; set; }

        public string SeatsName { get; set; }

        public string[] SlipLineSeparators { get; set; } = {};

        public Template[] Templates { get; set; } = {};
    }
}
