namespace TK302FBPrinter.Business.Models
{
    public class Seat
    {
        public int Row { get; set; }

        public int Place { get; set; }
    }
    
    public class TicketCinema
    {
        public string TemplateName { get; set; }

        public string Theatre { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string OGRN { get; set; }

        public string INN { get; set; }

        public string Movie { get; set; }

        public string Format { get; set; }

		public string License { get; set; }

		public string Age { get; set; }

		public string ShowDateTime { get; set; }

		public string Hall { get; set; }

		public string Price { get; set; }

		public string Discount { get; set; }

		public string PrintCode { get; set; }

		public string GiftCertificate { get; set; }

		public string BonusCard { get; set; }

		public string BonusType { get; set; }

		public string PaymentMethod { get; set; }

		public string PaymentDateTime { get; set; }
		
        public string Cashier { get; set; }

		public string Tax { get; set; }
        
		public string Comment { get; set; }

        public Seat[] Seats { get; set; }
    }
}
