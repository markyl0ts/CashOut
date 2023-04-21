using CashOut.Models;
using CashOut.Repository.Interfaces;
using System.Data.SqlClient;

namespace CashOut.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly SqlRepository _sqlRepository;
        public TransactionRepository(SqlRepository sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }

        public object Add(Transaction transaction)
        {
            string sql = "INSERT INTO [Transaction](ContactId, RateRangeId, [Reference], [Amount]) " +
                "VALUES(@contactId,@rateRangeId,@reference,@amount)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@contactId", transaction.ContactId));
            parameters.Add(new SqlParameter("@rateRangeId", transaction.RateRangeId));
            parameters.Add(new SqlParameter("@reference", transaction.ReferenceCode));
            parameters.Add(new SqlParameter("@amount", transaction.Ammount));

            var obj = _sqlRepository.ExecScalar(sql, parameters.ToArray());
            return obj;
        }

        public Transaction GetById(long transId)
        {
            Transaction transaction = new Transaction();
            string sql = "SELECT * FROM [Transaction] WHERE [Id] = @transId";
            SqlParameter param = new SqlParameter("@transId", transId);

            using (SqlDataReader reader = _sqlRepository.ExecDataReader(sql, param))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    transaction.Id = reader.GetInt64(0);
                    transaction.GuidId = reader.GetGuid(1);
                    transaction.RateRangeId = reader.GetInt64(2);
                    transaction.ReferenceCode = reader.GetString(3);
                    transaction.Ammount = reader.GetDecimal(4);
                    transaction.CreatedDate = reader.GetDateTime(5);
                    transaction.Status = reader.GetInt16(6);
                }
            }

            return transaction;
        }

        public List<Transaction> GetRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
