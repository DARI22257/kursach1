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
        DbConnection connection;

        private employeesDB(DbConnection db)
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
                cmd.Parameters.Add(new MySqlParameter("jobtitle", employees.Jobtitle));
                cmd.Parameters.Add(new MySqlParameter("schedule", employees.Schedule));
                cmd.Parameters.Add(new MySqlParameter("phone", employees.Phone));

                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
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
                var command = connection.CreateCommand("select `id`, `name`, `jobtitle` schedule `phone` ");
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
                            jobtitle = dr.GetString("jobtitle");
                        if (!dr.IsDBNull(1))
                            schedule = dr.GetDateTime("shedule");
                        if (!dr.IsDBNull(1))
                            phone = dr.GetString("phone");
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
                var mc = connection.CreateCommand($"update `employees` set `name`=@name, `Jobtitle`=@Jobtitle,`schedule`=@schedule, `phone`=@phone,  where `id` = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("name", edit.name));
                mc.Parameters.Add(new MySqlParameter("jobtitle", edit.Jobtitle));
                mc.Parameters.Add(new MySqlParameter("schedule", edit.Schedule));
                mc.Parameters.Add(new MySqlParameter("phone", edit.Phone));

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
                var mc = connection.CreateCommand($"delete from `employees` where `id`,`name`,`jobtitle`,`schedule`,`phone` = {remove.ID}");
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
                db = new employeesDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
