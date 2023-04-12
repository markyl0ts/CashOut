﻿using CashOut.Models;

namespace CashOut.Repository.Interfaces
{
    public interface IContactRepository
    {
        public List<Contact> GetAll();
        public object Add(Contact contact);
        public int Update(Contact contact);
        public int Delete(int contactId);
        public Contact GetById(int contactId);

    }
}
