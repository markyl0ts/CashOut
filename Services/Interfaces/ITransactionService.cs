using CashOut.Models;

namespace CashOut.Services.Interfaces
{
    public interface ITransactionService
    {
        public Transaction Add(Transaction transaction);
        public List<Transaction> GetTransactionsByDate(DateTime startDate, DateTime endDate);
    }
}
