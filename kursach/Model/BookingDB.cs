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
    class BookingDB
    {
        DbConnection connection;

        private BookingDB(DbConnection db)
        {
            this.connection = db;
        }

        public bool Insert(Booking booking)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Booking` Values (0,@Room numbers,@Date start,@Date end,@Status);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Room numbers", booking.Numberroom));
                cmd.Parameters.Add(new MySqlParameter("Date start", booking.Datestart));
                cmd.Parameters.Add(new MySqlParameter("Date end", booking.Dateend));
                cmd.Parameters.Add(new MySqlParameter("Status", booking.Status));

                try
                {
                    
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        
                        booking.Id = id;
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

        internal List<Booking> SelectAll()
        {
            List<Booking> booking = new List<Booking>();
            if (connection == null)
                return booking;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("select `ID`, `Room numbers`, `Date start`,`Date end`,`Status`");
                try
                {
                    
                    MySqlDataReader dr = command.ExecuteReader();
                  
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        int roomnumbers = 0;
                        DateTime datestart = DateTime.Now;
                        DateTime dateend = DateTime.Now;
                        string status = string.Empty;


                        if (!dr.IsDBNull(1))
                            roomnumbers = dr.GetInt16("Room numbers");
                        if (!dr.IsDBNull(2))
                            datestart = dr.GetDateTime("Date start");
                        if (!dr.IsDBNull(3))
                            dateend = dr.GetDateTime("Date end");
                        if (!dr.IsDBNull(4))
                            status = dr.GetString("Status");

                        booking.Add(new Booking
                        {
                            Id = id,
                            Numberroom = roomnumbers,
                            Datestart = datestart,
                            Dateend = dateend,
                            Status = status,
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return booking;
        }

        internal bool Update(Booking edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Booking` set `Room numbers`=@room numbers, `Date start`=@date start where `id`,`Date end`=@date end,`Status`=@status, = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Room numbers", edit.Numberroom));
                mc.Parameters.Add(new MySqlParameter("Date start", edit.Datestart));
                mc.Parameters.Add(new MySqlParameter("Date end", edit.Dateend));
                mc.Parameters.Add(new MySqlParameter("Status", edit.Status));

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


        internal bool Remove(Booking remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Room numbers` Date start `Date end` `Status` = {remove.Id}");
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

        static BookingDB db;
        public static BookingDB GetDb()
        {
            if (db == null)
                db = new BookingDB(DbConnection.GetDbConnection());
            return db;
        }
    }
}
