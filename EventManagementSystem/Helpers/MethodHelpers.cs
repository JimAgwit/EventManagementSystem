using EventManagementSystem.Models;
using MySql.Data.MySqlClient;

namespace EventManagementSystem.Helpers
{
    public class MethodHelpers
    {
        private readonly MySqlConnection _connection;
        public MethodHelpers(MySqlConnection connection)
        {
            _connection= connection;
        }
        public async Task<Customers> GetCustId(int id)
        {
         
            var customer = new Customers();
            var query = "SELECT * FROM customers WHERE Id = @Id";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@Id", id);
 
            await using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    customer.Id = reader.GetInt32("id");            
                    return customer;
                }
            }
         
            return null;
        }
    }
}
