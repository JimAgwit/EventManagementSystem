using EventManagementSystem.Helpers;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace EventManagementSystem.Controllers
{
    public class EventAssignmentController : Controller
    {
        private readonly MethodHelpers _methodHelpers;
        private readonly MySqlConnection _connection;
        public EventAssignmentController(MySqlConnection connection, MethodHelpers methodHelpers)
        {
            _connection = connection;
            _methodHelpers = methodHelpers;
        }

        public async Task<IActionResult> Index()
        {
            var events = new List<Event>();

        _connection.Open();
            await using (var command = new MySqlCommand("sp_GetAllAssignedEvents", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var eventList = new Event
                        {
                            Id = reader.GetInt32(0),
                            EventName = reader.GetString(1),
                            Description = reader.GetString(2),
                            Fullname = reader.GetString(3)
                        };

                        if (eventList == null)
                        {
                            return NotFound(); // Returns a 404 Not Found status code
                        }
                        events.Add(eventList);

                    }
                }
            }
             _connection.Close();

            return View(events);
        }
        public IActionResult AssignEventUi()
        {
            ViewBag.ActiveEventsList = _methodHelpers.GetActiveEventForDropdown();
            ViewBag.CustomerListForEvent = _methodHelpers.GetCustomerListForDropdown();
            return View();
        }


        public IActionResult SaveAssignedEvent(Event events)
        {

            //check for maximum booking
            _connection.Open();
            MySqlCommand command = new MySqlCommand("sp_CheckMaxBooking", _connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CustomerId", events.CustomerId);
            command.Parameters.AddWithValue("@EventId", events.Id);
            var count = Convert.ToInt32(command.ExecuteScalar());
            _connection.Close();
            if (count >= 5)
            {
                ModelState.AddModelError("", "You have reached the maximum number of bookings.");
            
                return View("ExceededBookingPage");
               
            }
            else
            {
                _connection.Open();
                MySqlCommand commands = new MySqlCommand("sp_SaveAssignedEvent", _connection);
                commands.CommandType = CommandType.StoredProcedure;
                commands.Parameters.AddWithValue("@EventId", events.Id);
                commands.Parameters.AddWithValue("@CustomerId", events.CustomerId);
                commands.ExecuteNonQuery();
                _connection.Close();

              
            }
            return RedirectToAction("Index");
        }
         
    }
}
