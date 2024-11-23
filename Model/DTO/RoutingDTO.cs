namespace MASAR.Model.DTO
{
    public class RoutingDTO
    {
        public string RouteName { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int TotalDistance { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public List<StopPointDTO> StopPoints { get; set; } = new List<StopPointDTO>();
    }
}