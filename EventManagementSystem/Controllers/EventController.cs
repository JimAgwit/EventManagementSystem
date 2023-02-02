using EventManagementSystem.Helpers;
using EventManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace EventManagementSystem.Controllers
{

    public class EventController : Controller
    {
        private readonly MySqlConnection _connection;

        public EventController(MySqlConnection connection)
        {
            _connection = connection;
        }
        // GET: EventController

        public async Task<IActionResult> Index()
        {
            var eventList = new List<Event>();

            _connection.Open();
            await using (var command = new MySqlCommand("sp_GetAllEvents", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var events = new Event
                        {
                            Id = reader.GetInt32(0),
                            EventName = reader.GetString(1),
                            Description = reader.GetString(2),
                            StartDate = reader.GetDateTime(3),
                            EndDate = reader.GetDateTime(4)

                        };

                        if (events == null)
                        {
                            return NotFound(); // Returns a 404 Not Found status code
                        }
                        eventList.Add(events);

                    }
                }
            }
            _connection.Close();

            return View(eventList);
        }


        //Expired events
        public async Task<IActionResult> GetExpiredEvents()
        {
            var eventList = new List<Event>();

            _connection.Open();
            await using (var command = new MySqlCommand("sp_GetExpiredEvents", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var events = new Event
                        {
                            Id = reader.GetInt32(0),
                            EventName = reader.GetString(1),
                            Description = reader.GetString(2),
                            StartDate = reader.GetDateTime(3),
                            EndDate = reader.GetDateTime(4)

                        };

                        if (events == null)
                        {
                            return NotFound(); // Returns a 404 Not Found status code
                        }
                        eventList.Add(events);

                    }
                }
            }
            _connection.Close();

            return View(eventList);
        }

        public IActionResult CreateEventUi()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveEvent(Event events)
        {
            if (events.StartDate > events.EndDate)
            {
                ModelState.AddModelError("", "Check Date Values. Start date must not be greater than end date");
                return View("CreateEventUi");
            }
            _connection.Open();
            MySqlCommand command = new MySqlCommand("sp_SaveEvent", _connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EventName", events.EventName);
            command.Parameters.AddWithValue("@Description", events.Description);
            command.Parameters.AddWithValue("@StartDate", events.StartDate);
            command.Parameters.AddWithValue("@EndDate", events.EndDate);
            command.ExecuteNonQuery();
            _connection.Close();

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> EditEventDetails(int id)
        {
             _connection.Open();
            var query = "SELECT * FROM events WHERE id = @eventId";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@eventId", id);

            await using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var events = new Event
                    {
                        Id = reader.GetInt32("id"),
                        EventName = reader.GetString("EventName"),
                        Description = reader.GetString("Description"),
                        StartDate = reader.GetDateTime("StartDate"),
                        EndDate = reader.GetDateTime("EndDate")
                    };
                     _connection.Close();
                    return View(events);
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> GetEventDetails(int id)
        {
             _connection.Open();
            var query = "SELECT * FROM events WHERE id = @eventId";
            var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@eventId", id);

            await using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var events = new Event
                    {
                        Id = reader.GetInt32("id"),
                        EventName = reader.GetString("EventName"),
                        Description = reader.GetString("Description"),
                        StartDate = reader.GetDateTime("StartDate"),
                        EndDate = reader.GetDateTime("EndDate")
                    };
                     _connection.Close();
                    return View(events);
                }
            }
            return NotFound();
        }


        public async Task<IActionResult> UpdateEvent(Event events)
        {
             _connection.Open();

            using (var command = new MySqlCommand("sp_UpdateEvent", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@eventId", events.Id);
                command.Parameters.AddWithValue("@EventName", events.EventName);
                command.Parameters.AddWithValue("@Description", events.Description);
                command.Parameters.AddWithValue("@StartDate", events.StartDate);
                command.Parameters.AddWithValue("@EndDate", events.EndDate);


                await command.ExecuteNonQueryAsync();
            }
             _connection.Close();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteEvent(Event events)
        {
             _connection.Open();

            using (var command = new MySqlCommand("sp_DeleteEvent", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@eventId", events.Id);

                await command.ExecuteNonQueryAsync();
            }
             _connection.Close();

            return RedirectToAction(nameof(Index));
        }


    }
}
