namespace CashOut.Models.ViewModels
{
    public class CashoutConfirmation
    {
        public long ContactId { get; set; }
        public string? ContactName { get; set; }
        public decimal ContactBalance { get; set; }
        public decimal CashoutAmount { get; set; }
        public decimal CashoutFee { get; set; }
    }
}
