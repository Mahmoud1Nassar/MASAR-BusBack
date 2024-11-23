namespace MASAR.Model.DTO
{
    public class BusLocationDTO
    {
        public string BusLocationId { get; set; }
        public string BusId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? PreviuseLatitude { get; set; }
        public double? PreviuseLongitude { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
