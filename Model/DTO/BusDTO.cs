namespace MASAR.Model.DTO
{
    public class BusDTO
    {
        public string BusId { get; set; }
        public int PlateNumber { get; set; }
        public DateTime LicenseExpiry { get; set; }
        public int CapacityNumber { get; set; }
        public string Status { get; set; }
        public string CurrentLocation { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}