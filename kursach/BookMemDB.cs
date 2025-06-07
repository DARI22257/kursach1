//using MySqlConnector;
//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace kursach
//{
//    public class BookMemDB
//    {
//        private readonly DBConnection dbconnection;

//        private BookMemDB(DBConnection db)
//        {
//            dbconnection = db;
//        }

//        public List<Guest> SearchGuests(string search)
//        {
//            List<Guest> result = new();

//            string query = "SELECT `Id`, `Firstname`, `Surname`, `Lastname`, `Phone`, `Email`, `Passportdata` FROM `Guest` " +
//                           "WHERE CONCAT(`Firstname`, ' ', `Surname`, ' ', `Lastname`) LIKE @search OR `Phone` LIKE @search";

//            if (dbconnection.OpenConnection())
//            {
//                using var cmd = dbconnection.CreateCommand(query);
//                cmd.Parameters.Add(new MySqlParameter("search", $"%{search}%"));

//                try
//                {
//                    using var reader = cmd.ExecuteReader();
//                    while (reader.Read())
//                    {
//                        var guest = new Guest
//                        {
//                            Id = reader.GetInt32("Id"),
//                            FirstName = reader.IsDBNull(0) ? "" : reader.GetString("Firstname"),
//                            Surname = reader.IsDBNull(1) ? "" : reader.GetString("Surname"),
//                            Lastname = reader.IsDBNull(2) ? "" : reader.GetString("Lastname"),
//                            Phone = reader.IsDBNull(3) ? "" : reader.GetString("Phone"),
//                            Email = reader.IsDBNull(4) ? "" : reader.GetString("Email"),
//                            Passportdata = reader.IsDBNull(5) ? "" : reader.GetString("Passportdata")
//                        };

//                        result.Add(guest);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    System.Windows.MessageBox.Show("Ошибка при поиске гостей: " + ex.Message);
//                }

//                dbconnection.CloseConnection();
//            }

//            return result;
//        }

//        // Singleton
//        private static BookMemDB db;
//        public static BookMemDB GetDb()
//        {
//            if (db == null)
//                db = new BookMemDB(DBConnection.GetDbConnection());
//            return db;
//        }

//        static BookMemDB bookMemDB;
//        private BookMemDB(DbConnection dbConnection)
//        {
//            this.dbconnection = dbconnection;
//        }
//        public static BookMemDB GetTable()
//        {
//            if (bookMemDB == null)
//                bookMemDB = new BookMemDB(DbConnection.GetDbConnection());
//            return bookMemDB;
//        }

//    }
//}
