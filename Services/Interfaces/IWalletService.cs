using CashOut.Models;
using CashOut.Models.Http;

namespace CashOut.Services.Interfaces
{
    public interface IWalletService
    {
        public Wallet GetById(long walletId);
        public Wallet GetByContact(long contactId);
        public object Add(Wallet wallet);
        public int UpdateBalance(WalletBalance walletBalance);
    }
}
