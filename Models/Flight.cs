namespace FlightStatusAPI.Models
{
    public class Flight
    {
        public int ID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTimeOffset Departure { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public FlightStatus Status { get; set; }
    }

    public enum FlightStatus
    {
        InTime,
        Delayed,
        Cancelled
    }
}
