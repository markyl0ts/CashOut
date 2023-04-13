using CashOut.Models;

namespace CashOut.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        public object Add(Transaction transaction);
        public Transaction GetById(long transId);
        public List<Transaction> GetRange(DateTime startDate, DateTime endDate);

    }
}
