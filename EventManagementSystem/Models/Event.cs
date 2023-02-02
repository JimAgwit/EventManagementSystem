using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EventManagementSystem.Models
{
    public class Event : BaseModel
    {
        public string EventName { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [Required]
     
        public DateTime EndDate { get; set; }
    }
}
