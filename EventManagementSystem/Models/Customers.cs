namespace EventManagementSystem.Models
{
    public class Customers :BaseModel
    {
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string? Gender { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
