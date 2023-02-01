
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Data;

namespace EventManagementSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly MySqlConnection _connection;
     
        public CustomerController(MySqlConnection connection)
        {
            _connection = connection;
           
        }
        public async Task<IActionResult> Index()
        {
            var customers = new List<Customers>();

            await _connection.OpenAsync();
            await using (var command = new MySqlCommand("sp_GetAllCustomers", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
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
                            DateCreated = reader.GetDateTime(6),
                        };

                        if (customer == null)
                        {
                            return NotFound(); // Returns a 404 Not Found status code
                        }
                        customers.Add(customer);
                     
                    }
                }
            }
            await _connection.CloseAsync();

            return View(customers);
        }

        //get deleted
        public async Task<IActionResult> GetDeletedCustomers()
        {
            var customers = new List<Customers>();

            await _connection.OpenAsync();
            await using (var command = new MySqlCommand("sp_GetDeletedCustomers", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
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
                            DateCreated = reader.GetDateTime(6),
                        };

                        if (customer == null)
                        {
                            return NotFound(); // Returns a 404 Not Found status code
                        }
                        customers.Add(customer);
                    }
                }
            }
            await _connection.CloseAsync();

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
                await _connection.OpenAsync();
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
                await _connection.CloseAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditCustomerDetails(int id)
        {

            await _connection.OpenAsync();
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
                    await _connection.CloseAsync();
                    return View(customer);
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> GetCustomerDetails(int id)
        {
            await _connection.OpenAsync();
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
                    await _connection.CloseAsync();
                    return View(customer);
                }
            }
            return NotFound();
        }


        public async Task<IActionResult> UpdateCustomer(Customers customer)
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

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateCustomerForDelete(Customers customer)
        {

            await _connection.OpenAsync();

            using (var command = new MySqlCommand("sp_UpdateCustomerForDelete", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@customerid", customer.Id);

                await command.ExecuteNonQueryAsync();
            }
            await _connection.CloseAsync();

            return RedirectToAction(nameof(Index));
        }



       
    }

}

