using kursachModel;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Windows;

namespace kursach.Model
{
    internal class NumberDB
    {
        DBConnection connection;

        private NumberDB(DBConnection db)
        {
            connection = db;
        }

        public List<NumberModel> SelectAll()
        {
            List<NumberModel> numbers = new List<NumberModel>();
            if (connection == null)
                return numbers;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT `ID`, `Numberroom`, `Type`, `Status`, `Price` FROM Number");
                try
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        numbers.Add(new NumberModel
                        {
                            Id = reader.GetInt32("ID"),
                            Numberroom = reader.GetInt32("Numberroom"),
                            Type = reader.GetString("Type"),
                            Status = reader.GetString("Status"),
                            Price = reader.GetInt32("Price")
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            connection.CloseConnection();
            return numbers;
        }

        static NumberDB db;
        public static NumberDB GetDb()
        {
            if (db == null)
                db = new NumberDB(DBConnection.GetDbConnection());
            return db;
        }

        public bool Insert(NumberModel number)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand(
                    "INSERT INTO Number (`Numberroom`, `Type`, `Status`, `Price`) VALUES (@Numberroom, @Type, @Status, @Price);"
                );

                cmd.Parameters.Add(new MySqlParameter("Numberroom", number.Numberroom));
                cmd.Parameters.Add(new MySqlParameter("Type", number.Type));
                cmd.Parameters.Add(new MySqlParameter("Status", number.Status));
                cmd.Parameters.Add(new MySqlParameter("Price", number.Price));

                try
                {
                    cmd.ExecuteNonQuery();
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

        public bool Update(NumberModel number)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand(
                    "UPDATE Number SET `Numberroom`=@Numberroom, `Type`=@Type, `Status`=@Status, `Price`=@Price WHERE `ID`=@Id"
                );

                cmd.Parameters.Add(new MySqlParameter("Numberroom", number.Numberroom));
                cmd.Parameters.Add(new MySqlParameter("Type", number.Type));
                cmd.Parameters.Add(new MySqlParameter("Status", number.Status));
                cmd.Parameters.Add(new MySqlParameter("Price", number.Price));
                cmd.Parameters.Add(new MySqlParameter("Id", number.Id));

                try
                {
                    cmd.ExecuteNonQuery();
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

        public bool Remove(NumberModel number)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var cmd = connection.CreateCommand("DELETE FROM Number WHERE `ID`=@id");
                cmd.Parameters.Add(new MySqlParameter("id", number.Id));

                try
                {
                    cmd.ExecuteNonQuery();
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
    }
}
