namespace EventManagementSystem.Models
{
    public class Contacts : BaseModel
    {
        public int CustomerId { get; set; }
        public string? Phone { get; set; }
        public string? Fullname { get; set; }
    }

  
}
