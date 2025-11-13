namespace CinemaSeatBooking.Models
{
    public class Movie
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public int Duration { get; set; }
        public string PosterUrl { get; set; } = "";
        public List<Showtime> Showtimes { get; set; } = new();
    }
}
