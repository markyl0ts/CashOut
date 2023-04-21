using CashOut.Models;

namespace CashOut.Services.Interfaces
{
    public interface ICashOutService
    {
        public SystemConfig GetSystemConfig(long configId);
        public List<RateRange> GetRates(long rateId);
        public bool IsValidAmount(decimal amount, decimal balance);
        public long GetRateRangeIdByAmount(decimal amount, List<RateRange> rates);
        public decimal GetRateFee(decimal amount, List<RateRange> rates);
    }
}
