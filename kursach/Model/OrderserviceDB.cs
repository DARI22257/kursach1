using kursach.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursach
{
    class OrderserviceDB
    {
        DBConnection connection;
        private OrderserviceDB(DBConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Orderservice orderservice)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Orderservice` Values (0,@Data);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Data", orderservice.Data));

                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());

                        orderservice.ID = id;
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

        internal List<Orderservice> SelectAll()
        {
            List<Orderservice> orderservice = new List<Orderservice>();
            if (connection == null)
                return orderservice;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Data` from Orderservice");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        DateTime data = DateTime.Now;

                        if (!dr.IsDBNull(1))
                            data = dr.GetDateTime("Data");


                        orderservice.Add(new Orderservice
                        {
                            ID = id,
                            Data = data,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return orderservice;
        }

        internal bool Update(Orderservice edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Orderservice` set `Date`=@date, = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("Data", edit.Data));

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


        internal bool Remove(Orderservice remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Date`  = {remove.ID}");
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

        static OrderserviceDB db;
        public static OrderserviceDB GetDb()
        {
            if (db == null)
                db = new OrderserviceDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}
