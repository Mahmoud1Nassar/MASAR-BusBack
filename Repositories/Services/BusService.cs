using MASAR.Data;
using MASAR.Model;
using MASAR.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using MASAR.Data;
using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace MASAR.Repositories.Services
{
    public class BusService : IBus
    {
        private readonly MASARDBContext _context;
        // inject jwt service
        public BusService(MASARDBContext context)
        {
            _context = context;
        }
        public async Task<List<BusWithDriverDTO>> GetAllBusesWithDriverName()
        {
            return await _context.Bus
                .Include(b => b.DriverProfiles)
                .Select(b => new BusWithDriverDTO
                {
                    BusId = b.BusId,
                    PlateNumber = b.PlateNumber,
                    LicenseExpiry = b.LicenseExpiry,
                    CapacityNumber = b.CapacityNumber,
                    Status = b.Status,
                    CurrentLocation = b.CurrentLocation,
                    UpdatedTime = b.UpdatedTime,
                    DriverName = b.DriverProfiles != null ? b.DriverProfiles.User.UserName : "No Driver Assigned"
                })
                .ToListAsync();
        }
        public async Task<List<StopPointWithCurrentLocation>> GetAllBusesLocations()
        {
            // Fetch all routes with their stop points
            var routingDetails = await _context.Routing
                .Include(r => r.StopPoints) // Include stop points for each route
                .ToListAsync();
            // Fetch all buses with current locations and driver information
            var buses = await _context.Bus
                .Include(b => b.DriverProfiles) // Include driver profiles
                .ThenInclude(dp => dp.User) // Include user details
                .Select(b => new
                {
                    b.BusId, // Include BusId
                    b.CurrentLocation,
                    b.DriverProfiles.DriverId,
                    DriverName = b.DriverProfiles.User.UserName // Assuming UserName is the driver's name
                })
                .ToListAsync();
            // Combine data into the DTO format
            var results = buses.Select(bus =>
            {
                // Match the route based on stop points or fallback to a default match
                var route = routingDetails.FirstOrDefault(r =>
                    r.StopPoints.Any(sp => sp.Name == bus.CurrentLocation))
                    ?? routingDetails.FirstOrDefault(r => r.RouteName != null);
                return new StopPointWithCurrentLocation
                {
                    RouteName = route?.RouteName ?? "No Route Assigned",
                    CurrentLocation = bus.CurrentLocation ?? "Unknown",
                    Driver = bus.DriverName, // Assign the driver's name
                    BusId = bus.BusId // Assign the BusId
                };
            }).ToList();
            return results;
        }
        public async Task<Bus> CreateBusInfo(BusDTO busDTO)
        {
            Bus bus = new Bus
            {
                BusId = busDTO.BusId,
                PlateNumber = busDTO.PlateNumber,
                LicenseExpiry = busDTO.LicenseExpiry,
                CapacityNumber = busDTO.CapacityNumber,
                Status = busDTO.Status,
                CurrentLocation = "LTUC",
                UpdatedTime = DateTime.Now
            };
             _context.Bus.Add(bus);
            await _context.SaveChangesAsync();
            return bus;
        }
        public async Task<StopPointWithCurrentLocation> CurrentLocationByDriverId(string driverId)
        {
            var driverRoute = await _context.Schedule
                .Where(a => a.DriverId == driverId)
                .FirstOrDefaultAsync();
   
            var routeDetails = await _context.Routing
                .Where(a => a.RoutingId == driverRoute.RoutingId)
                .Select(a => new
                {
                    a.RouteName,
                    StopPoints = a.StopPoints
                })
                .FirstOrDefaultAsync();

            var driverProfileBus = await _context.DriverProfile
                .Where(a => a.DriverId == driverId)
                .FirstOrDefaultAsync();

            var currentLocation = await _context.Bus
                .Where(a => a.BusId == driverProfileBus.BusId)
                .Select(a => a.CurrentLocation)
                .FirstOrDefaultAsync();

            return new StopPointWithCurrentLocation
            {
                CurrentLocation = currentLocation,
                StopPoints = routeDetails.StopPoints.ToList(),
                RouteName = routeDetails.RouteName
            };
        }

        public async Task<Bus> UpdateCurrentLocationByDriverId(string driverId, string Currentlocation)
        {
            var DriverBus = _context.DriverProfile.Where(a => a.DriverId == driverId)
                           .Select(a => a.BusId).FirstOrDefault();
            var BusUpdate = _context.Bus.Where(a => a.BusId == DriverBus).FirstOrDefault();
            BusUpdate.CurrentLocation = Currentlocation;
            _context.Entry(BusUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return BusUpdate;
        }
    }
}