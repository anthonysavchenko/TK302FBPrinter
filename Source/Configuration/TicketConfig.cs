namespace TK302FBPrinter.Configuration
{
    public class Line
    {
        public int PositionX1 { get; set; }

        public int PositionY1 { get; set; }

        public int PositionX2 { get; set; }

        public int PositionY2 { get; set; }

        public int Width { get; set; }
    }
    public class TextLine
    {
        public string Text { get; set; }

        public int Rotation { get; set; }

        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public int FontSize { get; set; }

        public int ScaleX { get; set; }
        
        public int ScaleY { get; set; }

        public int FontStyle { get; set; }
    }

    public class QrCode
    {
        public string Text { get; set; }

        public int Rotation { get; set; }

        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public int Scale { get; set; }
    }

    public class Bitmap
    {
        public int BitmapId { get; set; }

        public int Rotation { get; set; }

        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public int ScaleX { get; set; }
        
        public int ScaleY { get; set; }
    }

    public class Template
    {
        public string TemplateName { get; set; }
        
        public int SizeX { get; set; }

        public int SizeY { get; set; }

        public Line[] Lines { get; set; }

        public TextLine[] TextLines { get; set; }
        
        public TextLine[] SeatTextLines { get; set; }

        public QrCode[] QrCodes { get; set; }

        public Bitmap[] Bitmaps { get; set; }
    }

    public class TicketConfig
    {
        public const string SectionName = "Ticket";

        public string SeatsPlaceholder { get; set; }

        public string SeatsSeparator { get; set; }

        public string SeatsRowPlaceholder { get; set; }

        public string SeatsPlacePlaceholder { get; set; }

        public string SeatsName { get; set; }

        public Template[] Templates { get; set; }
    }
}
