using FlightStatusAPI.Models;

namespace FlightStatusAPI.Abstractions
{
    public interface IFlightRepository
    {
        List<Flight> GetFlights(string origin = null, string destination = null);
        Flight GetFlightById(int id);
        void AddFlight(Flight flight);
        void UpdateFlight(Flight flight);
        void DeleteFlight(int id);
    }
}
