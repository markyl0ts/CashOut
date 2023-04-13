using CashOut.Models;

namespace CashOut.Repository.Interfaces
{
    public interface IWalletRepository
    {
        public Wallet GetById(long walletId);
        public Wallet GetByContactId(long ContactId);
        public object Add(Wallet wallet);
        public int UpdateBalance(Wallet wallet);
    }
}
