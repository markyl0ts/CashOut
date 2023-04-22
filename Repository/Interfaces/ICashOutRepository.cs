using CashOut.Models;

namespace CashOut.Repository.Interfaces
{
    public interface ICashOutRepository
    {
        public SystemConfig GetSystemConfig(long configId);
        public Rate GetRate(long rateId);
        public List<RateRange> GetRates(long rateId);
        public int UpdateKioskBalanceAndAccumulatedAmount(long configId, decimal balance, decimal accumulatedAmount);

    }
}
