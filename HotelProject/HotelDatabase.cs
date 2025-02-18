using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using NPOI.HSSF.Record;

namespace HotelProject
{
    public class HotelDatabase
    {
        public string ConnectionString;

        public HotelDatabase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void CreateDatabaseAndTables()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string gueststable = @"CREATE TABLE IF NOT EXISTS guests
                                        (id INT PRIMARY KEY,
                                        first_name NVARCHAR(20) NOT NULL,
                                        last_name  NVARCHAR(30) NOT NULL,
                                        UCN NVARCHAR(10) NOT NULL UNIQUE,
                                        phone_number  NVARCHAR(15) NOT NULL);";

                string roomTypestable = @"CREATE TABLE IF NOT EXISTS room_types
                                            (id INT PRIMARY KEY,
                                            description  NVARCHAR(80) NOT NULL,
                                            max_capacity INT NOT NULL DEFAULT 2);";

                string roomstable = @"CREATE TABLE IF NOT EXISTS rooms
                                        (id INT PRIMARY KEY,
                                        number INT NOT NULL,
                                        status NVARCHAR(15) NOT NULL,
                                        price DECIMAL NOT NULL,
                                        room_type_id INT,
                                        FOREIGN KEY (room_type_id) REFERENCES room_types(id));";

                string reservationstable = @"CREATE TABLE IF NOT EXISTS reservations (
                                                id INTEGER PRIMARY KEY,
                                                accommodation_date DATE NOT NULL,
                                                release_date DATE NOT NULL,
                                                days INT NOT NULL,
                                                room_id INT,
                                                guest_id INT,
                                                FOREIGN KEY (room_id) REFERENCES rooms(id),
                                                FOREIGN KEY (guest_id) REFERENCES guests(id));";

                InsertIntoTables(ConnectionString);
            }
        }

        public void InsertIntoTables(string conn)
        {
            string insertguests = @"INSERT INTO guests (id, first_name, last_name, UCN, phone_number) VALUES
                                (1, 'David', 'Hunter', '9612047655', '0049456234354'),
                                (2, 'Barry', 'Johnson', '8809146088', '003305654733'),
                                (3, 'Peter', 'McGuel', '9510264905', '0044200485439'),
                                (4, 'Barbara', 'Feng', '8905174490', '003304500238'),
                                (5, 'Susan', 'Keil', '9102227445', '0039755623365'),
                                (6, 'Fred', 'Rapier', '9503118723', '00301205773'),
                                (7, 'Mary', 'Johnson', '9507205587', '003328130168'),
                                (8, 'Patricia', 'Gray', '9211256577', '0049457583490');";

            string insertRoomtype = @"INSERT INTO room_types (id, description, max_capacity) VALUES
                                   (1, 'Double', 2),
                                   (2, 'DoubleLux', 2),
                                   (3, 'Triple', 3),
                                   (4, 'Studio', 3),
                                   (5, 'Apartment', 4);";

            string insertRooms = @"INSERT INTO rooms (id, number, status, price, room_type_id) VALUES
                               (1, 101, 'free', 65, 1),
                               (2, 102, 'busy', 80, 2),
                               (3, 103, 'cleaning up', 65, 1),
                               (4, 104, 'busy', 90, 3),
                               (5, 105, 'busy', 120, 5),
                               (6, 201, 'busy', 100, 4),
                               (7, 202, 'free', 90, 3),
                               (8, 203, 'cleaning up', 65, 1),
                               (9, 204, 'free', 80, 2),
                               (10, 205, 'busy', 65, 1),
                               (11, 206, 'free', 120, 5),
                               (12, 207, 'busy', 100, 4),
                               (13, 208, 'busy', 80, 2),
                               (14, 209, 'cleaning up', 65, 1);";

            string insertReserv = @"INSERT INTO reservations (id, accommodation_date, release_date, days, room_id, guest_id) VALUES
                                      (1, '2021-09-12', '2021-09-17', 5, 3, 5),
                                      (2, '2021-09-20', '2021-09-23', 3, 7, 2),
                                      (3, '2021-10-09', '2021-10-15', 6, 9, 1),
                                      (4, '2021-10-10', '2021-10-12', 2, 4, 8),
                                      (5, '2021-10-15', '2021-10-17', 2, 10, 3),
                                      (6, '2021-10-15', '2021-10-17', 2, 3, 3),
                                      (7, '2021-10-21', '2021-10-24', 3, 1, 6),
                                      (8, '2021-10-21', '2021-10-24', 3, 2, 6),
                                      (9, '2021-10-21', '2021-10-24', 3, 9, 6),
                                      (10, '2021-10-25', '2021-10-30', 5, 5, 4);";
        }
        public void SelectQueryRoomsByPrice()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT TOP 5 number, status, price FROM rooms WHERE price BETWEEN 80 AND 100 ORDER BY price DESC;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["number"]} {reader["status"]} {reader["price"]}");
                    }
                }
            }
        }
        public void SelectGuestsItaly()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT first_name, last_name FROM guests WHERE phone_number LIKE '0033%';";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["first_name"]} {reader["last_name"]}");
                    }
                }
            }
        }
        public void SelectMinRoom()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT status, MIN(price) AS 'минимална цена' FROM rooms GROUP BY status;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["status"]} {reader["минимална цена"]}");
                    }
                }
            }
        }

        public void SelectLongestReservation()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT TOP 1 accommodation_date, release_date, MAX(days) AS 'максимален брой дни' FROM reservations;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"{reader["accommodation_date"]} {reader["release_date"]} {reader["максимален брой дни"]}");
                    }
                }
            }
        }
        public void SelectPriceAndCountFromRoom()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT price, COUNT(id) AS 'брой стаи' FROM rooms GROUP BY price;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["price"]} {reader["брой стаи"]}");
                    }
                }
            }
        }
        public void SelectTypeOfRoom(string roomType)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT r.number, r.status FROM rooms r JOIN room_types rt ON r.room_type_id = rt.id WHERE rt.description = @roomType;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@roomType", roomType);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["number"]} {reader["status"]}");
                        }
                    }
                }
            }
        }
        public void SelectBusyReservation()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT r.accommodation_date, r.release_date FROM reservations r JOIN rooms rm ON r.room_id = rm.id WHERE rm.status = 'busy';";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["accommodation_date"]} {reader["release_date"]}");
                    }
                }
            }
        }
        public void SelectGuestsWithMoreThan100Price()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT g.first_name, g.last_name, r.days, rm.number FROM guests g JOIN reservations r ON g.id = r.guest_id JOIN rooms rm ON r.room_id = rm.id WHERE rm.price > 100 ORDER BY g.first_name, g.last_name;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["first_name"]} {reader["last_name"]} {reader["days"]} {reader["number"]}");
                    }
                }
            }
        }
        public void SelectRoomStatus(string status)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT number, price FROM rooms WHERE status = @status;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["number"]}  {reader["price"]}");
                        }
                    }
                }
            }
        }
        public void SelectGuestUCN(string ucn)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT first_name, last_name FROM guests WHERE UCN = @ucn;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ucn", ucn);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"{reader["first_name"]} {reader["last_name"]}");
                        }
                    }
                }
            }
        }
        public void SelectReservationsDays(int days)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT COUNT(*) AS 'broi' FROM reservations WHERE days > @days;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@days", days);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"{reader["days"]} {reader["broi"]}");
                        }
                    }
                }
            }
        }

        public void SelectRoomsWithCapacity(int capacity)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT r.number, rt.description FROM rooms r JOIN room_types rt ON r.room_type_id = rt.id WHERE rt.max_capacity = @capacity;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@capacity", capacity);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["number"]} {reader["description"]}");
                        }
                    }
                }
            }
        }
        public void SelectReservationsWithName(string firstName, string lastName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT r.accommodation_date, r.release_date FROM reservations r JOIN guests g ON r.guest_id = g.id WHERE g.first_name = @firstName AND g.last_name = @lastName;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["accommodation_date"]} {reader["release_date"]}");
                        }
                    }
                }
            }
        }
        public void SelectReservationWithDates(string startDate, string endDate)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"SELECT g.first_name, g.last_name, r.room_id  FROM reservations r JOIN guests g ON r.guest_id = g.id WHERE r.accommodation_date BETWEEN @startDate AND @endDate;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($" {reader["first_name"]} {reader["last_name"]} {reader["room_id"]}");
                        }
                    }
                }
            }
        }
    }
}


