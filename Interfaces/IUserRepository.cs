using FlightStatusControlAPI.Models;

namespace FlightStatusControlAPI.Interfaces
{
    public interface IUserRepository 
    {
        User Login(string username, string password);
    }
}
