using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Windows;

namespace kursach.Model
{
    internal class employeesDB
    {
        DBConnection connection;

        private employeesDB(DBConnection db)
        {
            connection = db;
        }

        public bool Insert(employees employees)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("INSERT INTO `employees` VALUES (0, @name, @Jobtitle, @Schedule, @Phone); SELECT LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("name", employees.name));
                cmd.Parameters.Add(new MySqlParameter("Jobtitle", employees.Jobtitle));
                cmd.Parameters.Add(new MySqlParameter("Schedule", employees.Schedule));
                cmd.Parameters.Add(new MySqlParameter("Phone", employees.Phone));

                try
                {
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        employees.Id = id;
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

        internal List<employees> SelectAll()
        {
            List<employees> employees = new List<employees>();
            if (connection == null)
                return employees;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT `ID`, `name`, `Jobtitle`, `Schedule`, `Phone` FROM `employees`");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32("ID");
                        string name = dr.GetString("name");
                        string jobtitle = dr.GetString("Jobtitle");
                        DateTime schedule = dr.GetDateTime("Schedule");
                        string phone = dr.GetString("Phone");

                        employees.Add(new employees
                        {
                            Id = id,
                            name = name,
                            Jobtitle = jobtitle,
                            Schedule = schedule,
                            Phone = phone,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            connection.CloseConnection();
            return employees;
        }

        internal bool Update(employees edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand(
                    "UPDATE `employees` SET `name`=@name, `Jobtitle`=@jobtitle, `Schedule`=@Schedule, `Phone`=@phone WHERE `ID`=@id"
                );

                mc.Parameters.Add(new MySqlParameter("name", edit.name));
                mc.Parameters.Add(new MySqlParameter("jobtitle", edit.Jobtitle));
                mc.Parameters.Add(new MySqlParameter("Schedule", edit.Schedule));
                mc.Parameters.Add(new MySqlParameter("phone", edit.Phone));
                mc.Parameters.Add(new MySqlParameter("id", edit.Id));

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

        internal bool Remove(employees remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand("DELETE FROM `employees` WHERE `ID` = @id");
                mc.Parameters.Add(new MySqlParameter("id", remove.Id));

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

        static employeesDB db;
        public static employeesDB GetDb()
        {
            if (db == null)
                db = new employeesDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}
