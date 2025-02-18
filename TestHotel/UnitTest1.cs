using HotelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer;
using System.Data.SqlClient;

namespace TestHotel
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        private HotelDatabase hoteldatabase;
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        [Test]
        public void TestInsertData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM guests;", conn);
                var guestCount = cmd.ExecuteScalar();
                Assert.AreEqual(8, guestCount);
                cmd = new SqlCommand("SELECT COUNT(*) FROM room_types;", conn);
                var roomTypeCount = cmd.ExecuteScalar();
                Assert.AreEqual(5, roomTypeCount);
                cmd = new SqlCommand("SELECT COUNT(*) FROM rooms;", conn);
                var roomCount = cmd.ExecuteScalar();
                Assert.AreEqual(14, roomCount);
                cmd = new SqlCommand("SELECT COUNT(*) FROM reservations;", conn);
                var reservationCount = cmd.ExecuteScalar();
                Assert.AreEqual(10, reservationCount);
            }
        }
        [Test]
        public void TestSelectQueryRoomsByPrice()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var insertRooms = @"INSERT INTO rooms (id, number, status, price, room_type_id) VALUES
                                (15, 301, 'free', 85, 1),
                                (16, 302, 'busy', 95, 2),
                                (17, 303, 'free', 75, 1);";
                SqlCommand cmd = new SqlCommand(insertRooms, conn);
                cmd.ExecuteNonQuery();
                hoteldatabase.SelectQueryRoomsByPrice();
                cmd = new SqlCommand("SELECT COUNT(*) FROM rooms WHERE price BETWEEN 80 AND 100;", conn);
                var roomCount = cmd.ExecuteScalar();
                Assert.AreEqual(2, roomCount); 
            }
        }

        [Test]
        public void TestSelectGuestsItaly()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var insertGuest = @"INSERT INTO guests (id, first_name, last_name, UCN, phone_number) VALUES (9, 'Mario', 'Rossi', '1234567890', '0033123456789');";
                SqlCommand cmd = new SqlCommand(insertGuest, conn);
                cmd.ExecuteNonQuery();
                hoteldatabase.SelectGuestsItaly();
                cmd = new SqlCommand("SELECT COUNT(*) FROM guests WHERE phone_number LIKE '0033%';", conn);
                var guestCount = cmd.ExecuteScalar();
                Assert.AreEqual(2, guestCount); 
            }
        }

        [Test]
        public void TestSelectMinRoom()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var insertGuest = @"INSERT INTO guests (id, first_name, last_name, UCN, phone_number) VALUES (9, 'Mario', 'Rossi', '1234567890', '0033123456789');";
                SqlCommand cmd = new SqlCommand(insertGuest, conn);
                cmd.ExecuteNonQuery();
                hoteldatabase.SelectMinRoom();
                cmd = new SqlCommand("SELECT status, MIN(price) AS 'минимална цена' FROM rooms GROUP BY status;", conn);
                var price = cmd.ExecuteScalar();
                Assert.AreEqual(200, price);
            }
        }

        [Test]
        public void SelectLongestReservation()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var insertGuest = @"INSERT INTO guests (id, first_name, last_name, UCN, phone_number) VALUES (9, 'Mario', 'Rossi', '1234567890', '0033123456789');";
                SqlCommand cmd = new SqlCommand(insertGuest, conn);
                cmd.ExecuteNonQuery();
                hoteldatabase.SelectLongestReservation();
                cmd = new SqlCommand("SELECT TOP 1 accommodation_date, release_date, MAX(days) AS 'максимален брой дни' FROM reservations;", conn);
                var dates = cmd.ExecuteScalar();
                Assert.AreEqual("", dates);
            }
        }
        
    }
}
