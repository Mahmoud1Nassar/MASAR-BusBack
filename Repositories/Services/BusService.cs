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
            // Fetch all routes with schedules and stop points
            var routingDetails = await _context.Routing
                .Include(r => r.Schedules) // Include schedules
                .ThenInclude(s => s.User)  // Include user (driver)
                .ToListAsync();

            // Fetch all buses with driver profiles and current locations
            var buses = await _context.Bus
                .Include(b => b.DriverProfiles) // Include driver profiles
                .ThenInclude(dp => dp.User)     // Include driver user details
                .ToListAsync();

            // Map each route, bus, and driver into the DTO
            var results = routingDetails.Select(route =>
            {
                // Find the schedules related to this route
                var schedules = route.Schedules;

                // Map each schedule to its corresponding bus and driver details
                return schedules.Select(schedule =>
                {
                    // Find the bus driven by the driver in this schedule
                    var bus = buses.FirstOrDefault(dp => dp.DriverProfiles.DriverId == schedule.DriverId);

                    // Return the final DTO
                    return new StopPointWithCurrentLocation
                    {
                        CurrentLocation = bus?.CurrentLocation ?? "Unknown",
                        RouteName = route.RouteName ?? "No Route Assigned",
                        DriverName = bus?.DriverProfiles.User?.UserName ?? "Unknown",
                        PhoneNumber = bus?.DriverProfiles.User?.PhoneNumber ?? "Not Available",
                        BusId = bus?.BusId ?? "No Bus Assigned"
                    };
                }).ToList();
            })
            .SelectMany(x => x) // Flatten the nested lists into a single list
            .ToList();

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
            if (driverRoute == null)
                throw new Exception("Driver route not found");
            var routeDetails = await _context.Routing
                .Where(a => a.RoutingId == driverRoute.RoutingId)
                .Select(a => new
                {
                    a.RouteName,
                    a.StopPoints
                })
                .FirstOrDefaultAsync();
            if (routeDetails == null)
                throw new Exception("Route details not found");
            var driverProfileBus = await _context.DriverProfile
                .Where(a => a.DriverId == driverId)
                .Include(a => a.User)
                .FirstOrDefaultAsync();
            if (driverProfileBus == null)
                throw new Exception("Driver profile not found");
            var currentLocation = await _context.Bus
                .Where(a => a.BusId == driverProfileBus.BusId)
                .Select(a => a.CurrentLocation)
                .FirstOrDefaultAsync();
            if (currentLocation == null)
                throw new Exception("Current location not found");
            return new StopPointWithCurrentLocation
            {
                CurrentLocation = currentLocation,
                StopPoints = routeDetails.StopPoints?.ToList() ?? new List<StopPoint>(),
                RouteName = routeDetails.RouteName,
                DriverName = driverProfileBus.User?.UserName ?? "Unknown",
                PhoneNumber = driverProfileBus.User?.PhoneNumber ?? "Unknown",
                BusId = driverProfileBus.BusId
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