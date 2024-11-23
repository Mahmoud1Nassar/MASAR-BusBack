using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MASAR.Model
{
    public class Bus
    {
        public string BusId { get; set; }
        public int PlateNumber { get; set; }
        public DateTime LicenseExpiry {  get; set; }
        public int CapacityNumber { get; set; }
        public string Status { get; set; }
        public string CurrentLocation { get; set; }
        public DateTime UpdatedTime { get; set; }
        // Relationships
        public ICollection<Maintenance> Maintenances { get; set; }
        public BusLocation BusLocations { get; set; }
        public DriverProfile DriverProfiles { get; set; }
    }
}
