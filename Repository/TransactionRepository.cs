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
            string sql = "INSERT INTO [Transaction](SystemId, ContactId, RateRangeId, [Reference], [Amount]) " +
                "VALUES(@systemId,@contactId,@rateRangeId,@reference,@amount);SELECT CAST(scope_identity() AS bigint)";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@systemId", transaction.SystemId));
            parameters.Add(new SqlParameter("@contactId", transaction.ContactId));
            parameters.Add(new SqlParameter("@rateRangeId", transaction.RateRangeId));
            parameters.Add(new SqlParameter("@reference", transaction.ReferenceCode));
            parameters.Add(new SqlParameter("@amount", transaction.Amount));

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
                    transaction.SystemId = reader.GetInt64(2);
                    transaction.ContactId = reader.GetInt64(3);
                    transaction.RateRangeId = reader.GetInt64(4);
                    transaction.ReferenceCode = reader.GetString(5);
                    transaction.Amount = reader.GetDecimal(6);
                    transaction.CreatedDate = reader.GetDateTime(7);
                    transaction.Status = reader.GetInt16(8);
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
