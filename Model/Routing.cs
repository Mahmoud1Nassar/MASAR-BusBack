using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MASAR.Model
{
    public class Routing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RoutingId { get; set; } 
        public string RouteName { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public double TotalDistance { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<StopPoint> StopPoints { get; set; } = new List<StopPoint>();
        public ICollection<FavoriteRoute> FavoriteRoute { get; set; }
    }
}