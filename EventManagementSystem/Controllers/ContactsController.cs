using EventManagementSystem.Helpers;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace EventManagementSystem.Controllers
{
    public class ContactsController : Controller
    {
        private readonly MethodHelpers _methodHelpers;
        private readonly MySqlConnection _connection;

        public ContactsController(MySqlConnection connection, MethodHelpers methodHelpers)
        {
            _connection = connection;
            _methodHelpers = methodHelpers;
        }
        public async Task<IActionResult> Index()
        {
            var customers = new List<Contacts>();

            _connection.Open();
            await using (var command = new MySqlCommand("sp_GetContacts", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var contacts = new Contacts
                        {
                            Id = reader.GetInt32(0),
                            Fullname = reader.GetString(1),
                            Phone = reader.GetString(2),

                        };

                        if (contacts == null)
                        {
                            return NotFound(); // Returns a 404 Not Found status code
                        }
                        customers.Add(contacts);

                    }
                }
            }

            _connection.Close();

            return View(customers);
        }


        public IActionResult CreateNewContactUi()
        {
            ViewBag.CustomerLists = _methodHelpers.GetCustomerListForDropdown();
            return View();
        }


        public IActionResult SaveContact(int seletectedId, Contacts contacts)
        {
            _connection.Open();
            MySqlCommand command = new MySqlCommand("sp_SaveContact", _connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@customerId", contacts.CustomerId);
            command.Parameters.AddWithValue("@phone", contacts.Phone);
            command.ExecuteNonQuery();
            _connection.Close();

            return RedirectToAction("Index");
            
        }


        public async Task<IActionResult> EditContact(int id)
        {
            await _connection.OpenAsync();
            var query = "SELECT * FROM contacts WHERE Id = @id";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@id", id);

            await using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var contacts = new Contacts
                    {
                        Id = reader.GetInt32("id"),
                        Phone = reader.GetString("Phone")       
                    };
                    await _connection.CloseAsync();
                    return View(contacts);
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> UpdateContact(Contacts contact)
        {
            await _connection.OpenAsync();

            using (var command = new MySqlCommand("sp_UpdateContact", _connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@customerId", contact.Id);
                command.Parameters.AddWithValue("@phone", contact.Phone);
                await command.ExecuteNonQueryAsync();
            }
            await _connection.CloseAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeleteContact(Contacts contact)
        {
            await _connection.OpenAsync();

            using (var command = new MySqlCommand("sp_DeleteContact", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@contactId", contact.Id);

                await command.ExecuteNonQueryAsync();
            }
            await _connection.CloseAsync();

            return RedirectToAction(nameof(Index));
        }




    }

}


