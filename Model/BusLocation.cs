using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MASAR.Model
{
    public class BusLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BusLocationId { get; set; }
        public string BusId { get; set; }
        public Bus Bus {  get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? PreviuseLatitude { get; set; }
        public double? PreviuseLongitude { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
