using FlightStatusControlAPI.Interfaces;
using FlightStatusControlAPI.Models;
using FlightStatusControlAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace FlightStatusControlAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User Login(string login, string password)
        {
            string hash = password.CreateMD5();
            if (_dbContext.Users.Any(x => x.Username == login && x.Password == hash))
            {
                return _dbContext.Users.SingleOrDefault(x => x.Username == login && x.Password == hash);
            }
            return null;
        }
    }
}
