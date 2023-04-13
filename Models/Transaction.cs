namespace CashOut.Models
{
    public class Transaction : CashOutBase
    {
        public long ContactId { get; set; }
        public long RateRangeId { get; set; }
        public string? ReferenceCode { get; set; }
        public decimal Ammount { get; set; }
        public DateTime CreatedDate { get; set; }
        public short Status { get; set; }

    }
}
