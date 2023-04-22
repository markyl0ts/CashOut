namespace CashOut.Models
{
    public class SystemConfig : CashOutBase
    {
        public string? Name { get; set; }
        public decimal Balance { get; set; }
        public long RateId { get; set; }
        public decimal AccumulatedAmount { get; set; }
    }
}
