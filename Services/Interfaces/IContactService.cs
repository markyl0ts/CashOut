using CashOut.Models;

namespace CashOut.Services.Interfaces
{
    public interface IContactService
    {
        public List<Contact> GetAll();
        public Contact GetById(long id);
        public object Add(Contact contact);
        public int Delete(long id);
        public int Update(Contact contact);
        public Contact GetByPhone(string phone);
    }
}
