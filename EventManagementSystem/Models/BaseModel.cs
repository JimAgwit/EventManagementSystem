namespace EventManagementSystem.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? Fullname { get; set; }
        public int BookingCount { get; set; }
    }
}
