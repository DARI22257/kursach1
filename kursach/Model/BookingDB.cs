using kursachModel;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Windows;

namespace kursach
{
    public class BookingDB
    {
        private readonly DBConnection dbConnection;

        private static BookingDB instance;
        public static BookingDB GetDb()
        {
            if (instance == null)
                instance = new BookingDB(DBConnection.GetDbConnection());
            return instance;
        }

        private BookingDB(DBConnection connection)
        {
            this.dbConnection = connection;
        }

        public List<Booking> SelectAll()
        {
            var result = new List<Booking>();

            if (dbConnection.OpenConnection())
            {
                string query = "SELECT * FROM Booking";
                using (var cmd = dbConnection.CreateCommand(query))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Booking
                        {
                            Id = reader.GetInt32("Id"),
                            GuestId = reader.GetInt32("GuestId"),
                            RoomId = reader.GetInt32("RoomId"),
                            Datestart = reader.GetDateTime("Datestart"),
                            Dateend = reader.GetDateTime("Dateend"),
                            Status = reader.GetString("Status")
                        });
                    }
                }

                dbConnection.CloseConnection();
            }

            return result;
        }

        public bool Insert(Booking booking)
        {
            bool result = false;

            if (dbConnection.OpenConnection())
            {
                string query = "INSERT INTO Booking (GuestId, RoomId, Datestart, Dateend, Status) " +
                               "VALUES (@guestId, @roomId, @datestart, @dateend, @status)";

                using (var cmd = dbConnection.CreateCommand(query))
                {
                    cmd.Parameters.Add(new MySqlParameter("guestId", booking.GuestId));
                    cmd.Parameters.Add(new MySqlParameter("roomId", booking.RoomId));
                    cmd.Parameters.Add(new MySqlParameter("datestart", booking.Datestart));
                    cmd.Parameters.Add(new MySqlParameter("dateend", booking.Dateend));
                    cmd.Parameters.Add(new MySqlParameter("status", booking.Status));

                    result = cmd.ExecuteNonQuery() > 0;
                }

                dbConnection.CloseConnection();
            }

            return result;
        }

        public bool Update(Booking booking)
        {
            bool result = false;

            if (dbConnection.OpenConnection())
            {
                string query = "UPDATE Booking SET GuestId=@guestId, RoomId=@roomId, Datestart=@datestart, " +
                               "Dateend=@dateend, Status=@status WHERE Id=@id";

                using (var cmd = dbConnection.CreateCommand(query))
                {
                    cmd.Parameters.Add(new MySqlParameter("guestId", booking.GuestId));
                    cmd.Parameters.Add(new MySqlParameter("roomId", booking.RoomId));
                    cmd.Parameters.Add(new MySqlParameter("datestart", booking.Datestart));
                    cmd.Parameters.Add(new MySqlParameter("dateend", booking.Dateend));
                    cmd.Parameters.Add(new MySqlParameter("status", booking.Status));
                    cmd.Parameters.Add(new MySqlParameter("id", booking.Id));

                    result = cmd.ExecuteNonQuery() > 0;
                }

                dbConnection.CloseConnection();
            }

            return result;
        }

        public bool Remove(Booking booking)
        {
            bool result = false;

            if (dbConnection.OpenConnection())
            {
                try
                {
                    var cmd = dbConnection.CreateCommand("DELETE FROM Booking WHERE Id = @id");
                    cmd.Parameters.Add(new MySqlParameter("id", booking.Id));
                    result = cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка удаления: " + ex.Message);
                }

                dbConnection.CloseConnection();
            }

            return result;
        }
    }
}