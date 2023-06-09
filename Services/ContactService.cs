﻿using CashOut.Models;
using CashOut.Repository.Interfaces;
using CashOut.Services.Interfaces;

namespace CashOut.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public List<Contact> GetAll()
        {
            return _contactRepository.GetAll();
        }

        public Contact GetById(long id)
        {
            return _contactRepository.GetById(id);
        }

        public object Add(Contact contact)
        {
            return _contactRepository.Add(contact);
        }

        public int Delete(long id)
        {
            return _contactRepository.Delete(id);
        }

        public int Update(Contact contact)
        {
            return _contactRepository.Update(contact);
        }

        public Contact GetByPhone(string phone)
        {
            return _contactRepository.GetByPhone(phone);
        }
    }
}
