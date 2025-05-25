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
        DBConnection connection;

        private BookingDB(DBConnection db)
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
                MySqlCommand cmd = connection.CreateCommand("insert into `Booking` Values (0,@Numberroom,@Datestart,@Dateend,@Status);select LAST_INSERT_ID();");

                cmd.Parameters.Add(new MySqlParameter("Numberroom", booking.Numberroom));
                cmd.Parameters.Add(new MySqlParameter("Datestart", booking.Datestart));
                cmd.Parameters.Add(new MySqlParameter("Dateend", booking.Dateend));
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
                var command = connection.CreateCommand("select b.`ID`, n.`Numberroom`, b.`Datestart`,b.`Dateend`,b.`Status` from `Booking` b join `Number` n on n.`ID` = b.`IDroom`");
                try
                {
                    
                    MySqlDataReader dr = command.ExecuteReader();
                  
                    while (dr.Read())
                    {
                        int id = dr.GetInt32(0);
                        int numberroom = 0;
                        DateTime datestart = DateTime.Now;
                        DateTime dateend = DateTime.Now;
                        string status = string.Empty;


                        if (!dr.IsDBNull(1))
                            numberroom = dr.GetInt16("Numberroom");
                        if (!dr.IsDBNull(2))
                            datestart = dr.GetDateTime("Datestart");
                        if (!dr.IsDBNull(3))
                            dateend = dr.GetDateTime("Dateend");
                        if (!dr.IsDBNull(4))
                            status = dr.GetString("Status");

                        booking.Add(new Booking
                        {
                            Id = id,
                            Numberroom = numberroom,
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
                var mc = connection.CreateCommand($"update `Booking` set `Numberroom`=@numberroom, `Datestart`=@datestart where `id`,`Dateend`=@dateend,`Status`=@status, = {edit.Id}");
                mc.Parameters.Add(new MySqlParameter("Numberroom", edit.Numberroom));
                mc.Parameters.Add(new MySqlParameter("Datestart", edit.Datestart));
                mc.Parameters.Add(new MySqlParameter("Dateend", edit.Dateend));
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
                var mc = connection.CreateCommand($"delete from `Booking` where `id` = {remove.Id}");
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
                db = new BookingDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}
