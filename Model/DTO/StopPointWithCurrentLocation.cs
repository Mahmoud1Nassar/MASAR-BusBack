namespace MASAR.Model.DTO
{
    public class StopPointWithCurrentLocation
    {
        public string CurrentLocation { get; set; }
        public string RouteName { get; set; }
        public string Driver { get; set; }
        public string BusId { get; set; }
        public List<StopPoint> StopPoints { get; set; }
    }
}