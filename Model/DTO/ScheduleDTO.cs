namespace MASAR.Model.DTO
{
    public class ScheduleDTO
    {
        public string DriverId { get; set; }
        public string RoutingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime EstimatedTime { get; set; }
        public string Status { get; set; }
    }
}
