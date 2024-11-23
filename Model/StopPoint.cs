using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MASAR.Model
{
    public class StopPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StopPointId { get; set; }
        public string Name { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public string RoutingId { get; set; }
        public Routing Routing { get; set; }
    }
}