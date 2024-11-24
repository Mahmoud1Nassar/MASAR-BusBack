using MASAR.Model.DTO;
using MASAR.Model;

namespace MASAR.Repositories.Interfaces
{
    public interface IStudent
    {
        public Task<UserDTO> Register(SignUpDTO SignUpDTO);
        public Task<List<AnnouncementDTO>> ViewAnnouncements();
        public Task<List<Routing>> GetAllRoutes();
        public Task<List<ScheduleDTO>> GetAllSchedules();
        public Task<List<StopPoint>> GetAllStopPoints();
        public Task<EditUserDTO> EditUserProfile(string email, EditUserDTO edit);
        public Task<ApplicationUser> GetUserById(string Id);
        public Task<List<string>> GetAllFavioretRoutForUser(string UserId);
        public Task<FavoriteRoute> AddRoutForUser(string StudentId, string RoutId);
        public Task<FavoriteRoute> RemoveRoutForUser(string StudentId, string RoutId);
        public Task<List<Routing>> SearchRouteByName(string routeName);
    }
}