using EventManagementSystem.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace EventManagementSystem.Helpers
{
    public class MethodHelpers
    {
        private readonly MySqlConnection _connection;
        public MethodHelpers(MySqlConnection connection)
        {
            _connection = connection;
        }

        public List<Customers> GetCustomerListForDropdown()
        {
            List<Customers> customers = new List<Customers>();

            _connection.Open();

            using (MySqlCommand command = new MySqlCommand("sp_GetCustomerLists", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("Id");
                        string name = reader.GetString("Fullname");
                        customers.Add(new Customers
                        {
                            Id = id,
                            Fullname = name
                        });
                    }
                }
                _connection.Close();
                return customers;
            }
         
        }

        public List<Event> GetActiveEventForDropdown()
        {
            List<Event> events = new List<Event>();

            _connection.Open();

            using (MySqlCommand command = new MySqlCommand("sp_GetActiveEvents", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("Id");
                        string eventName = reader.GetString("EventName");
                        events.Add(new Event
                        {
                            Id = id,
                            EventName = eventName
                        });
                    }
                }
                _connection.Close();
                return events;
            }

        }

     
    }

}

