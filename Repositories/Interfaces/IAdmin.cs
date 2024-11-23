using MASAR.Model.DTO;
using MASAR.Model;
namespace MASAR.Repositories.Interfaces
{
    public interface IAdmin
    {
        public Task<UserDTO> GetUserByEmail(string email);
        public Task<bool> DeleteUser(string email);
        public Task<ApplicationUser> CreateUser(SignUpDTO user);
        public Task<List<ApplicationUser>> GetAllDrivers();
        public Task<List<ApplicationUser>> GetAllUsers();
        public Task<DashboardData> GetDashboardDataAsync();
        public Task<Announcement> CreateAnnouncement(string email, AnnouncementDTO AnnouncementDto);
        public Task<List<Maintenance>> GetAllMaintenanceRequests();
        public Task<Maintenance> ApprovedMaintenanceRequest(string ifApprove, MaintenancaDTO maintenanceDTO);
        public Task<Routing> GetRoutingById(string routingId);
        public Task<Routing> CreateRouting(RoutingDTO routingDto);
        public Task<Schedule> CreateSchedule(ScheduleDTO scheduleDto);
        public Task<List<Schedule>> GetSchedulesByRouteId(string routeId);
        public Task<List<AnnouncementDTO>> ViewAnnouncementsForAdmin();
    }
}