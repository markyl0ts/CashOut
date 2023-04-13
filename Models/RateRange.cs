namespace CashOut.Models
{
    public class RateRange : CashOutBase
    {
        public long RateId { get; set; }
        public decimal StartRange { get; set; }
        public decimal EndRange { get; set; }
        public decimal Fee { get; set; }
    }
}
