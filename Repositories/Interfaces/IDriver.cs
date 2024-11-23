using MASAR.Model.DTO;
using MASAR.Model;

namespace MASAR.Repositories.Interfaces
{
    public interface IDriver 
    {
        public Task<DriverProfile> CreateInfo(string email, DriverInfoDTO driveInfo);
        public Task<DriverInfoDTO> UpdateDriver(string email, DriverInfoDTO driver);
        public Task<List<AnnouncementDTO>> ViewAnnouncements();
        public Task<Maintenance> MaintenanceRequest(string email, MaintenancaDTO maintenanceDTO);
    }
}