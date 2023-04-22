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

        public object Add(Contact contact)
        {
            string sql = "INSERT INTO Contact([guid],FullName, FirstName, LastName, MiddleName, Email, PhoneNo)" +
                "values(NEWID(), @fullName, @firstName, @lastName, @middleName, @email, @phone)";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@fullName", contact.FullName));
            sqlParameters.Add(new SqlParameter("@firstName", contact.FirstName));
            sqlParameters.Add(new SqlParameter("@lastName", contact.LastName));
            sqlParameters.Add(new SqlParameter("@middleName", contact.MiddleName));
            sqlParameters.Add(new SqlParameter("@email", contact.Email));
            sqlParameters.Add(new SqlParameter("@phone", contact.Phone));

            var obj = _sqlRepository.ExecScalar(sql, sqlParameters.ToArray());
            return obj;
        }

        public int Delete(long contactId)
        {
            string sql = "DELETE FROM Contact WHERE [id] = @id";
            SqlParameter param = new SqlParameter("@id", contactId);
            int res = _sqlRepository.ExecNonQuery(sql, param);
            return res;
        }

        public Contact GetById(long contactId)
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
            }

            return contact;
        }

        public List<Contact> GetAll()
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
            }

            return contacts;
        }

        public int Update(Contact contact)
        {
            string sql = "UPDATE Contact SET ";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(contact.FullName))
            {
                sql += "[FullName] = @fullName,";
                sqlParameters.Add(new SqlParameter("@fullName", contact.FullName));
            }

            if(!string.IsNullOrEmpty(contact.FirstName))
            {
                sql += "[FirstName] = @firstName,";
                sqlParameters.Add(new SqlParameter("@firstName", contact.FirstName));
            }

            if(!string.IsNullOrEmpty(contact.LastName))
            {
                sql += "[LastName] = @lastName,";
                sqlParameters.Add(new SqlParameter("@lastName", contact.LastName));
            }

            if(!string.IsNullOrEmpty(contact.MiddleName))
            {
                sql += "[MiddleName] = @middleName,";
                sqlParameters.Add(new SqlParameter("@MiddleName", contact.MiddleName));
            }

            if(!string.IsNullOrEmpty(contact.Email))
            {
                sql += "[Email] = @email,";
                sqlParameters.Add(new SqlParameter("@email", contact.Email));
            }

            if(!string.IsNullOrEmpty(contact.Phone))
            {
                sql += "[PhoneNo] = @phone,";
                sqlParameters.Add(new SqlParameter("@phone", contact.Phone));
            }

            sql += "WHERE [id] = @id";
            sqlParameters.Add(new SqlParameter("@id", contact.Id));
            sql = sql.Replace(",WHERE", " WHERE");

            int res = _sqlRepository.ExecNonQuery(sql, sqlParameters.ToArray());
            return res;
        }

        public Contact GetByPhone(string? phone)
        {
            Contact contact = new Contact();
            string sql = "SELECT * FROM Contact WHERE [PhoneNo] = @phoneNo";
            SqlParameter param = new SqlParameter("@phoneNo", phone);

            using (SqlDataReader reader = _sqlRepository.ExecDataReader(sql, param))
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
            }

            return contact;
        }
    }
}
