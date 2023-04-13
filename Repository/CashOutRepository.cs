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

                _sqlRepository.Dispose();
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
                        rateRange.RateId = reader.GetInt64(2);
                        rateRange.StartRange = reader.GetDecimal(3);
                        rateRange.EndRange = reader.GetDecimal(4);
                        rateRange.Fee = reader.GetDecimal(5);

                        rates.Add(rateRange);
                    }
                }

                _sqlRepository.Dispose();
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
                    config.Name = reader.GetString(2);
                    config.Balance = reader.GetDecimal(3);
                    config.RateId = reader.GetInt64(4);
                }

                _sqlRepository.Dispose();
            }

            return config;
        }
    }
}
