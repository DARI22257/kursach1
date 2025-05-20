using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                MySqlCommand cmd = connection.CreateCommand("insert into `employees` Values (0, @name, @Jobtitle,@Schedule, @Phone);select LAST_INSERT_ID();");

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
                var command = connection.CreateCommand("select `id`, `name`, `Jobtitle`,  `Schedule`,`Phone` from `employees` ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        string name = string.Empty;
                        string jobtitle = string.Empty;
                        DateTime schedule = DateTime.Now;
                        string phone = string.Empty;

                        if (!dr.IsDBNull(1))
                            name = dr.GetString("name");
                        if (!dr.IsDBNull(1))
                            jobtitle = dr.GetString("Jobtitle");
                        if (!dr.IsDBNull(1))
                            schedule = dr.GetDateTime("Schedule");
                        if (!dr.IsDBNull(1))
                            phone = dr.GetString("Phone");
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
                var mc = connection.CreateCommand($"update `employees` set `name`=@name, `jobtitle`=@Jobtitle,`schedule`=@Schedule, `phone`=@Phone,  where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("name", edit.name));
                mc.Parameters.Add(new MySqlParameter("Jobtitle", edit.Jobtitle));
                mc.Parameters.Add(new MySqlParameter("Schedule", edit.Schedule));
                mc.Parameters.Add(new MySqlParameter("Phone", edit.Phone));

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
                var mc = connection.CreateCommand($"delete from `employees` where `id`,`name`,`Jobtitle`,`Schedule`,`Phone` = {remove.Id}");
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
