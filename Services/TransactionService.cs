using CashOut.Models;
using CashOut.Repository.Interfaces;
using CashOut.Services.Interfaces;

namespace CashOut.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly Random _random;
        public TransactionService(ITransactionRepository transactionRepository, Random random)
        {
            _transactionRepository = transactionRepository;
            _random = random;
        }

        public Transaction Add(Transaction transaction)
        {
            int randNo = _random.Next();
            transaction.ReferenceCode = string.Format("{0}{1}", transaction.ContactId, randNo);

            Transaction trans = new Transaction();
            var obj = _transactionRepository.Add(transaction);
            if(obj != null)
                trans = _transactionRepository.GetById((long)obj);

            return trans;
        }

        public List<Transaction> GetTransactionsByDate(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

    }
}
