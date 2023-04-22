using CashOut.Models;
using CashOut.Repository.Interfaces;
using CashOut.Services.Interfaces;

namespace CashOut.Services
{
    public class CashOutService : ICashOutService
    {
        private readonly ICashOutRepository _cashOutRepository;

        public CashOutService(ICashOutRepository cashOutRepository)
        {
            _cashOutRepository = cashOutRepository;
        }

        public decimal GetRateFee(decimal amount, List<RateRange> rates)
        {
            decimal fee = 0;
            foreach (RateRange rateRange in rates)
            {
                if (amount > rateRange.StartRange && amount < rateRange.EndRange)
                    fee = rateRange.Fee; break;
            }

            return fee;
        }

        public long GetRateRangeIdByAmount(decimal amount, List<RateRange> rates)
        {
            long rateRangeId = 0;

            foreach(RateRange rateRange in rates)
            {
                if(amount > rateRange.StartRange && amount < rateRange.EndRange)
                    rateRangeId = rateRange.Id; break;
            }

            return rateRangeId;
        }

        public List<RateRange> GetRates(long rateId)
        {
            return _cashOutRepository.GetRates(rateId);
        }

        public SystemConfig GetSystemConfig(long configId)
        {
            return _cashOutRepository.GetSystemConfig(configId);
        }

        public bool IsValidAmount(decimal amount, decimal balance)
        {
            if(amount < 0)
                return false;

            if(balance < 0)
                return false;

            if(balance < amount)
                return false;

            return true;
        }

        public int UpdateKioskBalanceAndAccumulatedAmount(long configId, decimal balance, decimal accumulatedAmount)
        {
            return _cashOutRepository.UpdateKioskBalanceAndAccumulatedAmount(configId, balance, accumulatedAmount);
        }
    }
}
