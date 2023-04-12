using CashOut.Models;
using CashOut.Repository.Interfaces;
using System.Data.SqlClient;

namespace CashOut.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly SqlRepository _sqlRepository;
        public ContactRepository(SqlRepository sqlRepository) 
        {
            _sqlRepository = sqlRepository;
        }

        Contact IContactRepository.Add(Contact contact)
        {
            throw new NotImplementedException();
        }

        Contact IContactRepository.Delete(int contactId)
        {
            throw new NotImplementedException();
        }

        Contact IContactRepository.GetById(int contactId)
        {
            Contact contact = new Contact();
            string sql = "SELECT * FROM Contact WHERE [id] = @id";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@id", contactId));

            using (SqlDataReader reader = _sqlRepository.ExecDataReader(sql, sqlParameters.ToArray()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    contact.Id = reader.GetInt64(0);
                    contact.GuidId = reader.GetGuid(1);
                    contact.FullName = reader.GetString(2);
                    contact.FirstName = reader.GetString(3);
                    contact.LastName = reader.GetString(4);
                    contact.MiddleName = reader.GetString(5);
                    contact.Phone = reader.GetString(7);
                    contact.Status = reader.GetString(8);
                }

                _sqlRepository.Dispose();
            }

            return contact;
        }

        List<Contact> IContactRepository.GetAll()
        {
            List<Contact> contacts = new List<Contact>();
            string sql = "SELECT * FROM Contact";
            using(SqlDataReader reader = _sqlRepository.ExecDataReader(sql))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contact contact = new Contact();
                        contact.Id = reader.GetInt64(0);
                        contact.GuidId = reader.GetGuid(1);
                        contact.FullName = reader.GetString(2);
                        contact.FirstName = reader.GetString(3);
                        contact.LastName = reader.GetString(4);
                        contact.MiddleName = reader.GetString(5);
                        contact.Email = reader.GetString(6);
                        contact.Phone = reader.GetString(7);
                        contact.Status = reader.GetString(8);
                        contacts.Add(contact);
                    }
                }

                _sqlRepository.Dispose();
            }

            return contacts;
        }

        Contact IContactRepository.Update(Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}
