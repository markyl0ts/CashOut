using CashOut.Models;

namespace CashOut.Services.Interfaces
{
    public interface IContactService
    {
        public List<Contact> GetAll();
        public Contact GetById(int id);
    }
}
