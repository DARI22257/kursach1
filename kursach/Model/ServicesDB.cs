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
        DBConnection connection;
        private ServicesDB(DBConnection db)
        {
            this.connection = db;
        }

        public bool Insert(kursachModel.ServicesModel services)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Services` Values (0,@Title,@price);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Title", services.Title));
                cmd.Parameters.Add(new MySqlParameter("price", services.Price));

                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
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

        internal List<kursachModel.ServicesModel> SelectAll()
        {
            List<kursachModel.ServicesModel> services = new List<kursachModel.ServicesModel>();
            if (connection == null)
                return services;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Title`, `price` from `Services`");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        int price = 0;
                        string title = string.Empty;
                        
                        if (!dr.IsDBNull(1))
                            price = dr.GetInt16("price");
                        if (!dr.IsDBNull(2))
                            title = dr.GetString("Title");

                        services.Add(new kursachModel.ServicesModel
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

        internal bool Update(kursachModel.ServicesModel edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Services` set `Title`=@title,`price`=@price, = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Title", edit.Title));
                mc.Parameters.Add(new MySqlParameter("price", edit.Price));

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


        internal bool Remove(kursachModel.ServicesModel remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Services` where `ID` = {remove.Id}");
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
                db = new ServicesDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}
