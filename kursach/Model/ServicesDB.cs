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
    internal class ServicesDB
    {
        private ServicesDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Services services)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Services` Values (0,@Title,@Price);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Title", services.Title));
                cmd.Parameters.Add(new MySqlParameter("Price", services.Price));

                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());

                        services.Id = id;
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

        internal List<Services> SelectAll()
        {
            List<Services> number = new List<Services>();
            if (connection == null)
                return number;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Title`, `Price`");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        int price = 0;
                        string title = string.Empty;
                        
                        if (!dr.IsDBNull(1))
                            price = dr.GetInt16("Price");
                        if (!dr.IsDBNull(2))
                            title = dr.GetString("Title");

                        services.Add(new services
                        {
                            Id = id,
                            Title = title,
                            Price = price,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return services;
        }

        internal bool Update(Services edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Services` set `Title`=@title,`Price`=@price, = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("Price", edit.Price));

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


        internal bool Remove(Services remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from ` Title` Price` = {remove.Id}");
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

        static ServicesDB db;
        public static ServicesDB GetDb()
        {
            if (db == null)
                db = new ServicesDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
