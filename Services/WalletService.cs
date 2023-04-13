using CashOut.Models;
using CashOut.Models.Http;
using CashOut.Repository.Interfaces;
using CashOut.Services.Interfaces;

namespace CashOut.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository) 
        {
            _walletRepository = walletRepository;
        }

        public object Add(Wallet wallet)
        {
            return _walletRepository.Add(wallet);
        }

        public Wallet GetByContact(long contactId)
        {
            return _walletRepository.GetByContactId(contactId);
        }

        public Wallet GetById(long walletId)
        {
            return _walletRepository.GetById(walletId);
        }

        public int UpdateBalance(WalletBalance walletBalance)
        {
            Wallet wallet = new Wallet();
            wallet.Id = walletBalance.WalletId;
            wallet.Balance = walletBalance.Balance;

            return _walletRepository.UpdateBalance(wallet);
        }
    }
}
