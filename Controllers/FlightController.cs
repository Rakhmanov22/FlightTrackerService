using FlightStatusControlAPI.Interfaces;
using FlightStatusControlAPI.Models;
using FlightStatusControlAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightStatusControlAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<FlightController> _logger;

        public FlightController(IFlightRepository flightRepository, IMemoryCache cache, ILogger<FlightController> logger)
        {
            _flightRepository = flightRepository;
            _cache = cache;
            _logger = logger;   
        }

        [HttpGet]
        public IActionResult GetFlights([FromQuery]string? origin = null, string? destination = null)
        {
            var cacheKey = $"Flights_{origin}_{destination}";
            var username = User.Identity.Name;
            // Попробовать получить данные из кэша
            if (_cache.TryGetValue(cacheKey, out List<Flight> flights))
            {
                return Ok(flights);
            }

            // Если данных нет в кэше, получить из репозитория и добавить в кэш
            flights = _flightRepository.GetFlights(origin, destination);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Настройте срок действия кэша
            };

            _cache.Set(cacheKey, flights, cacheEntryOptions);

            return Ok(flights);
        }

        [HttpPost]
        [Authorize(Roles = "0")]
        public IActionResult AddFlight([FromBody] Flight flight)
        {
            try
            {
                var username = User.Identity.Name;
                // Валидация и добавление нового рейса
                _flightRepository.AddFlight(flight);

                // Очистить кэш после изменения данных в БД
                _cache.Remove("Flights");

                // Добавления логов в файл лог
                _logger.LogInformation("Add in the database by user {Username} at {Timestamp}", username, DateTime.UtcNow);
                return Ok("Flight added successfully");
            }
            catch(Exception ex)
            {
                _logger.LogError("Error in the database by user {Username} at {Timestamp}", ex.Message.ToString(), DateTime.UtcNow);
                return StatusCode(500, "An error occurred on the server. Please try again later.");
            }
            
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "0")]
        public IActionResult UpdateFlight(int id, [FromBody] FlightStatusUpdateModel statusUpdate)
        {
            try
            {
                var username = User.Identity.Name;
                // Проверка наличия рейса с указанным идентификатором
                var flight = _flightRepository.GetFlightById(id);

                if (flight == null)
                {
                    return NotFound(); // Возвращаем 404 Not Found, если рейс не найден
                }

                // Обновление данных рейса
                flight.Status = statusUpdate.Status;

                // Сохранение изменений в БД
                _flightRepository.UpdateFlight(flight);

                // Очистить кэш после изменения данных в БД
                _cache.Remove("Flights");

                // Добавления логов в файл лог
                _logger.LogInformation("Change in the database by user {Username} at {Timestamp}", username, DateTime.UtcNow);
                return Ok("Flight updated successfully");
            }            
            catch(Exception ex)
            {
                _logger.LogError("Error in the database by user {Message} at {Timestamp}", ex.Message.ToString(), DateTime.UtcNow);
                return StatusCode(500, "An error occurred on the server. Please try again later.");
            }
        }
    }
}