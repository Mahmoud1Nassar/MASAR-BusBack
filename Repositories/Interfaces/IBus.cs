
using MASAR.Model.DTO;
using MASAR.Model;

namespace MASAR.Repositories.Interfaces
{
    public interface IBus
    {
        public Task<Bus> CreateBusInfo(BusDTO busDTO);
        public Task<StopPointWithCurrentLocation> CurrentLocationByDriverId(string driverId);
        public Task<Bus> UpdateCurrentLocationByDriverId(string driverId, string Currentlocation);
        public Task<List<BusWithDriverDTO>> GetAllBusesWithDriverName();
        public Task<List<StopPointWithCurrentLocation>> GetAllBusesLocations();
    }
}