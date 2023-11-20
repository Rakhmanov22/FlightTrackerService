using FlightStatusControlAPI.Abstractions;
using FlightStatusControlAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FlightStatusControlAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/flights")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMemoryCache _cache;

        public FlightController(IFlightRepository flightRepository, IMemoryCache cache)
        {
            _flightRepository = flightRepository;
            _cache = cache;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetFlights(string origin = null, string destination = null)
        {
            var cacheKey = $"Flights_{origin}_{destination}";

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
        [AllowAnonymous]
        public IActionResult AddFlight([FromBody] Flight flight)
        {
            // Валидация и добавление нового рейса
            _flightRepository.AddFlight(flight);

            // Очистить кэш после изменения данных в БД
            _cache.Remove("Flights");

            return Ok("Flight added successfully");
        }
    }
}