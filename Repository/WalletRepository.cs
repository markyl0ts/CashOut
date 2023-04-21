using CashOut.Models;
using CashOut.Repository.Interfaces;
using System.Data.SqlClient;

namespace CashOut.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly SqlRepository _sqlRepository;
        public WalletRepository(SqlRepository sqlRepository)
        {
            _sqlRepository = sqlRepository;
        }

        public object Add(Wallet wallet)
        {
            string sql = "INSERT INTO Wallet(ContactId, [Balance]) VALUES(@contactId, @balance)";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@contactId", wallet.ContactId));
            sqlParameters.Add(new SqlParameter("@balance", wallet.Balance));

            var obj = _sqlRepository.ExecScalar(sql, sqlParameters.ToArray());
            return obj;
        }

        public Wallet GetByContactId(long ContactId)
        {
            Wallet wallet = new Wallet();
            string sql = "SELECT * FROM Wallet WHERE [ContactId] = @contactId";
            SqlParameter param = new SqlParameter("@contactId", ContactId);

            using (SqlDataReader reader = _sqlRepository.ExecDataReader(sql, param))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    wallet.Id = reader.GetInt64(0);
                    wallet.GuidId = reader.GetGuid(1);
                    wallet.ContactId = reader.GetInt64(2);
                    wallet.Balance = reader.GetDecimal(3);
                }
            }

            return wallet;
        }

        public Wallet GetById(long walletId)
        {
            Wallet wallet = new Wallet();
            string sql = "SELECT * FROM Wallet WHERE [id] = @id";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@id", walletId));

            using (SqlDataReader reader = _sqlRepository.ExecDataReader(sql, sqlParameters.ToArray()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    wallet.Id = reader.GetInt64(0);
                    wallet.GuidId = reader.GetGuid(1);
                    wallet.ContactId = reader.GetInt64(2);
                    wallet.Balance = reader.GetDecimal(3);
                }
            }

            return wallet;
        }

        public int UpdateBalance(Wallet wallet)
        {
            string sql = "UPDATE Wallet SET [Balance] = @balance WHERE [Id] = @id";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@balance", wallet.Balance));
            sqlParameters.Add(new SqlParameter("@id", wallet.Id));

            int res = _sqlRepository.ExecNonQuery(sql, sqlParameters.ToArray());
            return res;
        }
    }
}
