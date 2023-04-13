namespace CashOut.Models.Http
{
    public class CashOutRequest
    {
        public long SystemConfigId { get; set; }
        public string ContactNo { get; set; }
        public decimal Amount { get; set; }
    }
}
