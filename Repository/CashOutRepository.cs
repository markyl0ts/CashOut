using CashOut.Helpers;
using CashOut.Models;
using CashOut.Repository.Interfaces;
using System.Data.SqlClient;

namespace CashOut.Repository
{
    public class CashOutRepository : ICashOutRepository
    {
        private readonly SqlRepository _sqlRepository;
        public CashOutRepository(SqlRepository sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }

        public Rate GetRate(long rateId)
        {
            Rate rate = new Rate();
            string sql = "SELECT * FROM [Rate] WHERE [Id] = @id";
            SqlParameter param = new SqlParameter("@rateId", rateId);

            using (SqlDataReader reader = _sqlRepository.ExecDataReader(sql, param))
            {
                if(reader.HasRows)
                {
                    rate.Id = reader.GetInt64(0);
                    rate.GuidId = reader.GetGuid(1);
                    rate.Name = reader.GetString(2);
                }
            }

            return rate;
        }

        public List<RateRange> GetRates(long rateId)
        {
            List<RateRange> rates = new List<RateRange>();
            string sql = "SELECT * FROM [RateRange] WHERE [RateId] = @rateId";
            SqlParameter param = new SqlParameter("@rateId", rateId);

            using (SqlDataReader reader = _sqlRepository.ExecDataReader(sql, param))
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        RateRange rateRange = new RateRange();
                        rateRange.Id = reader.GetInt64(0);
                        rateRange.GuidId = reader.GetGuid(1);
                        rateRange.RateId = reader.GetSafeLong(2);
                        rateRange.StartRange = reader.GetSafeDecimal(3);
                        rateRange.EndRange = reader.GetSafeDecimal(4);
                        rateRange.Fee = reader.GetSafeDecimal(5);

                        rates.Add(rateRange);
                    }
                }
            }

            return rates;
        }

        public SystemConfig GetSystemConfig(long configId)
        {
            SystemConfig config = new SystemConfig();
            string sql = "SELECT * FROM [System] WHERE [Id] = @id";
            SqlParameter param = new SqlParameter("@id", configId);

            using (SqlDataReader reader = _sqlRepository.ExecDataReader(sql, param))
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    config.Id = reader.GetInt64(0);
                    config.GuidId = reader.GetGuid(1);
                    config.Name = reader.GetSafeString(2);
                    config.Balance = reader.GetSafeDecimal(3);
                    config.RateId = reader.GetSafeLong(4);
                    config.AccumulatedAmount = reader.GetSafeDecimal(5);
                }
            }

            return config;
        }

        public int UpdateKioskBalanceAndAccumulatedAmount(long configId, decimal balance, decimal accumulatedAmount)
        {
            string sql = "UPDATE [System] SET [Balance] = @balance, [AccumulatedAmount] = @amount WHERE Id = @configId";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@balance", balance));
            parameters.Add(new SqlParameter("@amount", accumulatedAmount));
            parameters.Add(new SqlParameter("@configId", configId));

            int res = _sqlRepository.ExecNonQuery(sql, parameters.ToArray());
            return res;
        }
    }
}
