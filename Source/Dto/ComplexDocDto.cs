using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TK302FBPrinter.Dto
{
    public enum BonusTypeDto
    {
        Pay,
        Accrual
    }

    public enum SeatTypeDto
    {
        Vip,
        Sofa,
        Simple,
        BeatBox,
        Sector1,
        Sector2,
        Sector3,
        Sector4,
        Sector5,
    }

    public enum ComplexDocPaymentTypeDto {

        Card,
        Bonus,

        [EnumMember(Value = "pushkin-card")]
        PushkinCard,

        [EnumMember(Value = "gift-card")]
        GiftCard
    }

    public class ComplexDocSeatDto
    {
        [Required]
        public int Row { get; set; }

        [Required]
        public int Place { get; set; }

        [Required]
        [EnumDataType(typeof(SeatTypeDto))]
        public SeatTypeDto Type { get; set; }

        [Required]
        public int Price { get; set; }

        [JsonProperty("ticket_number")]
        public string TicketNumber { get; set; }
    }

    public class ComplexDocTicketsDto
    {
        [Required]
        public bool Agent { get; set; }

        [Required]
        [JsonProperty("theater_name")]
        public string TheatreName { get; set; }

        [Required]
        [JsonProperty("theater_legal_name")]
        public string TheatreLegalName { get; set; }

        [Required]
        public string OGRN { get; set; }

        [Required]
        public string INN { get; set; }

        [Required]
        [JsonProperty("legal_address")]
        public string LegalAddress { get; set; }

        [Required]
        public string Movie { get; set; }

        [Required]
        public string Format { get; set; }

        [Required]
        public string License { get; set; }

        [Required]
        public string Age { get; set; }

        [Required]
        [JsonProperty("show_date")]
        public string ShowDate { get; set; }

        [Required]
        public string Hall { get; set; }

        [Required]
        public int Amount { get; set; }

        public int? Discount { get; set; }

        [Required]
        [JsonProperty("print_code")]
        public string PrintCode { get; set; }

        public string Certificate { get; set; }

        [JsonProperty("bonus_card")]
        public string BonusCard { get; set; }

        [JsonProperty("bonus_type")]
        [EnumDataType(typeof(BonusTypeDto))]
        public BonusTypeDto? BonusType { get; set; }

        [Required]
        [JsonProperty("payment_type")]
        [EnumDataType(typeof(ComplexDocPaymentTypeDto))]
        public ComplexDocPaymentTypeDto PaymentType { get; set; }

        [JsonProperty("pushkin_card_owner_name")]
        public string PushkinCardOwnerName { get; set; }

        [Required]
        public string Cashier { get; set; }

        [Required]
        [JsonProperty("payment_date")]
        public string PaymentDate { get; set; }

        [EnumDataType(typeof(ComplexDocTaxTypeDto))]
        public ComplexDocTaxTypeDto Tax { get; set; }

        public string Comment { get; set; }

        public string Email { get; set; }

        [Required]
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [Required]
        [JsonProperty("viewers_count")]
        public int ViewersCount { get; set; }

        [JsonProperty("agent_name")]
        public string AgentName { get; set; }

        [JsonProperty("agent_phone")]
        public string AgentPhone { get; set; }

        [Required]
        public ComplexDocSeatDto[] Seats { get; set; }

        public int? NDS { get; set; } 

        [JsonProperty("gift_card_number")]
        public string GiftCardNumber { get; set; }
    
        [JsonProperty("gift_card_summ")]
        public int? GiftCardAmount { get; set; }

        [JsonProperty("add_payment_summ")]
        public int? GiftCardExtraAmount { get; set; }
    }

    public enum ComplexDocTaxTypeDto
    {
        AutomaticMode,
        Traditional,
        LightIncome,
        LightIncomeNoExpenses,
        SingleTax,
        Agricultural,
        Patent
    }

    public class ItemDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public int Price { get; set; }
    }

    public class GoodDto
    {
        [Required]
        public bool Agent { get; set; }

        public string INN { get; set; }

        [JsonProperty("agent_name")]
        public string AgentName { get; set; }

        [JsonProperty("agent_phone")]
        public string AgentPhone { get; set; }

        [Required]
        public ItemDto[] Items { get; set; }

        [Required]
        public int Amount { get; set; }

        [EnumDataType(typeof(ComplexDocTaxTypeDto))]
        public ComplexDocTaxTypeDto Tax { get; set; }

        public string Comment { get; set; }

        public int? NDS { get; set; }

        [Required]
        public bool Refund { get; set; }
	}

    public class ComplexDocDto
    {
        [JsonProperty("operation_id")]
        public string OperationId { get; set; }

        public bool Reprint { get; set; }

        [JsonProperty("slip_check")]
        public string SlipCheck { get; set; }

        public ComplexDocTicketsDto Tickets { get; set; }

        public GoodDto Goods { get; set; }
    }
}
