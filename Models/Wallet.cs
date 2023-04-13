namespace CashOut.Models
{
    public class Wallet : CashOutBase
    {
        public long ContactId { get; set; }
        public decimal Balance { get; set; }
    }
}
