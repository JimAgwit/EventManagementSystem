using EventManagementSystem.Helpers;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace EventManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MySqlConnection _connection;
        private readonly MethodHelpers _methodHelpers;
        public CustomerController(MySqlConnection connection, MethodHelpers methodHelpers)
        {
            _connection = connection;
            _methodHelpers = methodHelpers;
        }



        public async Task<IActionResult> Index()
        {
            var customers = new List<Customers>();

            _connection.Open();
            await using (var command = new MySqlCommand("SELECT * FROM Customers", _connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var customer = new Customers
                        {
                            Id = reader.GetInt32(0),
                            Firstname = reader.GetString(1),
                            Middlename = reader.GetString(2),
                            Lastname = reader.GetString(3),
                            Birthday = reader.GetDateTime(4),
                            Gender = reader.GetString(5),
                        };
                        customers.Add(customer);
                    }
                }
            }
            _connection.Close();

            return View(customers);
        }

        public IActionResult CreateCustomerUi()
        {
            return View();
        }


        public async Task<IActionResult> CreateCustomer(Customers customers)
        {

            await using (var command = new MySqlCommand("sp_SaveCustomer", _connection))
            {
                _connection.Open();
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Firstname", customers.Firstname);
                command.Parameters.AddWithValue("@Middlename", customers.Middlename);
                command.Parameters.AddWithValue("@lastname", customers.Lastname);
                command.Parameters.AddWithValue("@Birthday", customers.Birthday);
                command.Parameters.AddWithValue("@Gender", customers.Gender);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //  Console.WriteLine(reader["column1"] + " " + reader["column2"]);
                    }
                }
                _connection.Close();
            }

            return RedirectToAction(nameof(Index));
        }

        //public async Task<Customers> GetCustomerId(int id)
        //{
        //    _connection.Open();
        //    await _methodHelpers.GetCustId(id);
        //    _connection.Close();
        //    return null;
        //}
        
        public async Task<IActionResult> EditCustomerDetails(int id)
        {
            _connection.Open();
            var custid = (await _methodHelpers.GetCustId(id));
            _connection.Close();

            _connection.Open();
            var query = "SELECT * FROM customers WHERE id = @id";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@id", id);
            
            await using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var customer = new Customers
                    {
                        Id = reader.GetInt32("id"),
                        Firstname = reader.GetString("Firstname"),
                        Middlename = reader.GetString("Middlename"),
                        Lastname = reader.GetString("Lastname"),
                        Gender = reader.GetString("Gender"),
                        Birthday = reader.GetDateTime("Birthday"),
                    };
                    _connection.Close();
                    return View(customer);           
                }        
            }     
            return NotFound();
        }

        public async Task<IActionResult> GetCustomerDetails(int id)
        {
            _connection.Open();
            var query = "SELECT * FROM customers WHERE id = @id";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@id", id);

            await using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var customer = new Customers
                    {
                        Id = reader.GetInt32("id"),
                        Firstname = reader.GetString("Firstname"),
                        Middlename = reader.GetString("Middlename"),
                        Lastname = reader.GetString("Lastname"),
                        Gender = reader.GetString("Gender"),
                        Birthday = reader.GetDateTime("Birthday"),
                    };
                    _connection.Close();
                    return View(customer);
                }
            }
            return NotFound();
        }


        public async Task UpdateCustomer(Customers customer, int id)
        {      
            await _connection.OpenAsync();
            using (var command = new MySqlCommand("sp_UpdateCustomer", _connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@customerid", customer.Id);
                command.Parameters.AddWithValue("@Firstname", customer.Firstname);
                command.Parameters.AddWithValue("@Middlename", customer.Middlename);
                command.Parameters.AddWithValue("@Lastname", customer.Lastname);
                command.Parameters.AddWithValue("@Gender", customer.Gender);
                command.Parameters.AddWithValue("@Birthday", customer.Birthday);

                await command.ExecuteNonQueryAsync();
            }
            await _connection.CloseAsync();
        }

    }
}

