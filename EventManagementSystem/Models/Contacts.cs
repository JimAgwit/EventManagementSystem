using Microsoft.Build.Framework;

namespace EventManagementSystem.Models
{
    public class Contacts : BaseModel
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Fullname { get; set; }
    }

  
}
