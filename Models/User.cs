using System.Data;

namespace FlightStatusControlAPI.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        // Роли для клиентов
        public Role Role { get; set; }
    }
}
