namespace CinemaSeatBooking.Models
{
    public class Showtime
    {
        public string Id { get; set; } = "";
        public DateTime StartAt { get; set; }
        public string AuditoriumId { get; set; } = "";
        public decimal UnitPrice { get; set; }
    }
}
