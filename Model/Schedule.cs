using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MASAR.Model
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ScheduleId { get; set; }
        public string DriverId { get; set; }
        public ApplicationUser User { get; set; }
        public string RoutingId { get; set; }
        public Routing Routing { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime EstimatedTime { get; set; }
        public string Status { get; set; }
    }
}