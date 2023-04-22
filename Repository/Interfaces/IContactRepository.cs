using CashOut.Models;

namespace CashOut.Repository.Interfaces
{
    public interface IContactRepository
    {
        public List<Contact> GetAll();
        public object Add(Contact contact);
        public int Update(Contact contact);
        public int Delete(long contactId);
        public Contact GetById(long contactId);
        public Contact GetByPhone(string phone);

    }
}
