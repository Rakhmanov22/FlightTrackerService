using FlightStatusControlAPI.Models;

namespace FlightStatusControlAPI.Interfaces
{
    public interface IFlightRepository
    {
        List<Flight> GetFlights(string origin = null, string destination = null);
        Flight GetFlightById(int id);
        Role GetRoleById(int id);
        void AddFlight(Flight flight);
        void UpdateFlight(Flight flight);
        void DeleteFlight(int id);
    }
}
