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
    internal class NumberDB
    {
        DBConnection connection;

        private NumberDB(DBConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Number number)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Number` Values (0,@Numberroom,@Type,@Status,@Price);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Numberroom", number.Numberroom));
                cmd.Parameters.Add(new MySqlParameter("Type", number.Type));
                cmd.Parameters.Add(new MySqlParameter("Status", number.Status));
                cmd.Parameters.Add(new MySqlParameter("Price", number.Price));

                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());

                        number.Id = id;
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

        internal List<Number> SelectAll()
        {
            List<Number> number = new List<Number>();
            if (connection == null)
                return number;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Numberroom`, `Type`,`Status`,`Price`");
                try
                {

                    MySqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        int numberroom = 0;
                        string type = string.Empty;
                        string status = string.Empty;
                        int price = 0;

                        if (!dr.IsDBNull(1))
                            numberroom = dr.GetInt16("Numberroom");
                        if (!dr.IsDBNull(2))
                            type = dr.GetString("Type");
                        if (!dr.IsDBNull(3))
                            status = dr.GetString("Status");
                        if (!dr.IsDBNull(4))
                            price = dr.GetInt16("Price");

                        number.Add(new Number
                        {
                            Id = id,
                            Numberroom = numberroom,
                            Type = type,
                            Status = status,
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
            return number;
        }

        internal bool Update(Number edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Number` set `Numberroom`=@numberroom, `Type`=@type where `id`,`Status`=@status,`Price`=@price, = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Numberroom", edit.Numberroom));
                mc.Parameters.Add(new MySqlParameter("Type", edit.Type));
                mc.Parameters.Add(new MySqlParameter("Status", edit.Status));
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


        internal bool Remove(Number remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Numberroom` Type `Status` Price ` = {remove.Id}");
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

        static NumberDB db;
        public static NumberDB GetDb()
        {
            if (db == null)
                db = new NumberDB(DBConnection.GetDBConnection());
            return db;
        }
    }
}
