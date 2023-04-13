using CashOut.Models;

namespace CashOut.Services.Interfaces
{
    public interface IContactService
    {
        public List<Contact> GetAll();
        public Contact GetById(int id);
        public object Add(Contact contact);
        public int Delete(int id);
        public int Update(Contact contact);
        public Contact GetByPhone(string phone);
    }
}
