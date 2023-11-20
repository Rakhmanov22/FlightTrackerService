using FlightStatusControlAPI.Abstractions;
using FlightStatusControlAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightStatusControlAPI.Data.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public FlightRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Flight> GetFlights(string origin = null, string destination = null)
        {
            var flightsQuery = _dbContext.Flights.AsQueryable();

            if (!string.IsNullOrEmpty(origin))
            {
                flightsQuery = flightsQuery.Where(f => f.Origin.Contains(origin));
            }

            if (!string.IsNullOrEmpty(destination))
            {
                flightsQuery = flightsQuery.Where(f => f.Destination.Contains(destination));
            }

            return flightsQuery.OrderBy(f => f.Arrival).ToList();
        }

        public Flight GetFlightById(int id)
        {
            return _dbContext.Flights.Find(id);
        }

        public void AddFlight(Flight flight)
        {
            _dbContext.Flights.Add(flight);
            _dbContext.SaveChanges();
        }

        public void UpdateFlight(Flight flight)
        {
            _dbContext.Entry(flight).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void DeleteFlight(int id)
        {
            var flight = _dbContext.Flights.Find(id);
            if (flight != null)
            {
                _dbContext.Flights.Remove(flight);
                _dbContext.SaveChanges();
            }
        }
    }
}
