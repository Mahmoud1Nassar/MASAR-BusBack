using MASAR.Data;
using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
namespace MASAR.Repositories.Services
{
    public class Driver : IDriver
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MASARDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // inject jwt service
        private JwtTokenService _jwtTokenService;
        public Driver(UserManager<ApplicationUser> Manager,
            SignInManager<ApplicationUser> signInManager, JwtTokenService jwtTokenService, MASARDBContext context)
        {
            _userManager = Manager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        public Task<List<AnnouncementDTO>> ViewAnnouncements()
        {
            var allAnnoun = _context.Announcement.Where(a => a.Audience == "Driver")
                 .Select(a => new AnnouncementDTO
                 {
                     Title = a.Title,
                     Content = a.Content,
                     CreatedTime = a.CreatedTime
                 })
                .ToListAsync();
            return allAnnoun;
        }
        public async Task<DriverProfile> CreateInfo(string email, DriverInfoDTO driveInfo)
        {
            var driverById = await _userManager.Users.Where(e => e.Id == driveInfo.DriverId).FirstOrDefaultAsync();

            if (driverById.Id == driveInfo.DriverId)
            {
                var result = new DriverProfile
                {
                    DriverId = driverById.Id,
                    LicenseNumber = driveInfo.LicenseNumber,
                    BusId = driveInfo.BusId,
                    Status = "Active"
                };
                var resultWithPass = _context.DriverProfile.Add(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
        public async Task<DriverInfoDTO> UpdateDriver(string email, DriverInfoDTO driver)
        {
            try
            {
                // Find the driver by ID
                var driverById = await _userManager.FindByIdAsync(driver.DriverId.ToString());
                // Find the driver's profile
                var driverProfile = await _context.DriverProfile
                                                  .SingleOrDefaultAsync(a => a.DriverId == driverById.Id);
              
                // Update the driver profile
                driverProfile.BusId = driver.BusId;
                driverProfile.LicenseNumber = driver.LicenseNumber;
                driverProfile.Status = driver.Status;
                // Mark the profile as modified
                _context.Entry(driverProfile).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return driver; // Return the updated driver info
            }
            catch (Exception e)
            {
                // Log or handle the exception
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<Maintenance> MaintenanceRequest(string email, MaintenancaDTO maintenanceDTO)
        {
            var driverById = await _context.DriverProfile.Where(e => e.BusId == maintenanceDTO.BusId).FirstOrDefaultAsync();
            var driver = await _userManager.FindByIdAsync(driverById.DriverId);
            if (driver.Email == email && maintenanceDTO.BusId == driverById.BusId)
            {
                var maintenance = new Maintenance
                {
                    DriverId = driverById.DriverProfileId,
                    BusId = driverById.BusId,
                    RequestDate = maintenanceDTO.RequestDate,
                    Description = maintenanceDTO.Description,
                    Status = maintenanceDTO.Status,
                    ExpectedMaintenanceDays = maintenanceDTO.ExpectedMaintenanceDays
                };
                _context.Maintenance.Add(maintenance);
                await _context.SaveChangesAsync();
                return maintenance;
            }
            else
            {
                return null;
            }
        }
    }
}