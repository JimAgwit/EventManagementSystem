using Microsoft.Build.Framework;

namespace EventManagementSystem.Models
{
    public class Customers :BaseModel
    {
        [Required]
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        [Required]
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        [Required]
        public string Gender { get; set; }
        public DateTime DateCreated { get; set; }
    }

 
}
