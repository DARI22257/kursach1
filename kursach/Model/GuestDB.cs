using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursach
{
    class GuestDB
    {
        DbConnection connection;

        private GuestDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Guest guest)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Guest` Values (0, @Name, @Surname, @Lastname, @Phone, @Email, @Passport data);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Name", guest.FirstName));
                cmd.Parameters.Add(new MySqlParameter("Surname", guest.Surname));
                cmd.Parameters.Add(new MySqlParameter("Lastname", guest.Lastname));
                cmd.Parameters.Add(new MySqlParameter("Phone", guest.Phone));
                cmd.Parameters.Add(new MySqlParameter("Email", guest.Email));
                cmd.Parameters.Add(new MySqlParameter("Passport data", guest.Passportdata));

                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        guest.Id = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Guest> SelectAll()
        {
            List<Guest> guests = new List<Guest>();
            if (connection == null)
                return guests;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `id`, `Name`, `Surname`,`Lastname`,`Phone`,`Email`,`Passport data`   ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string Name = string.Empty;
                        string Surname = string.Empty;
                        string Lastname = string.Empty;
                        string Phone = string.Empty;
                        string Email = string.Empty;
                        string Passportdata = string.Empty;

                        if (!dr.IsDBNull(1))
                            Name = dr.GetString("Name");
                        if (!dr.IsDBNull(2))
                            Surname = dr.GetString("Surname");
                        if (!dr.IsDBNull(3))
                            Lastname = dr.GetString("Lastname");
                        if (!dr.IsDBNull(4))
                            Phone = dr.GetString("Phone");
                        if (!dr.IsDBNull(5))
                            Email = dr.GetString("Email");
                        if (!dr.IsDBNull(1))
                            Passportdata = dr.GetString("Passportdata");

                        guests.Add(new Guest
                        {
                            Id = id,
                            FirstName = Name,
                            Surname = Surname,
                            Lastname = Lastname,
                            Phone = Phone,
                            Email = Email,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return guests;
        }

        internal bool Update(Guest edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Guest` set `Firstname`=@firstname, `Surname`=@surname where `id`, `Lastname`=@lastname,`Phone`=@phone,`Email`=@email,`Passportdata`=@passportdata = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Firstname", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("Surname", edit.Surname));
                mc.Parameters.Add(new MySqlParameter("Lastname", edit.Lastname));
                mc.Parameters.Add(new MySqlParameter("Phone", edit.Phone));
                mc.Parameters.Add(new MySqlParameter("Email", edit.Email));
                mc.Parameters.Add(new MySqlParameter("Passportdata", edit.Passportdata));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Guest remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Firstname` Surname` Lastname` Phone` Email` Passpotdata` = {remove.ID}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static GuestDB db;
        public static GuestDB GetDb()
        {
            if (db == null)
                db = new GuestDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
