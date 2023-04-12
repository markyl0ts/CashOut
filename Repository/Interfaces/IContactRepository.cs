using CashOut.Models;

namespace CashOut.Repository.Interfaces
{
    public interface IContactRepository
    {
        public List<Contact> GetAll();
        public Contact Add(Contact contact);
        public Contact Update(Contact contact);
        public Contact Delete(int contactId);
        public Contact GetById(int contactId);

    }
}
