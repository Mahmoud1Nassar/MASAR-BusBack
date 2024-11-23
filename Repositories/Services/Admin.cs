using MASAR.Data;
using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
namespace MASAR.Repositories.Services
{
    public class Admin : IAdmin
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MASARDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // inject jwt service
        private JwtTokenService _jwtTokenService;
        public Admin(UserManager<ApplicationUser> Manager,
            SignInManager<ApplicationUser> signInManager, JwtTokenService jwtTokenService, MASARDBContext context)
        {
            _userManager = Manager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            try
            {
                var allUSers = await _context.Users.ToListAsync();
                return allUSers;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<Schedule>> GetSchedulesByRouteId(string routeId)
        {
            return await _context.Schedule
                .Where(s => s.RoutingId == routeId)
                .ToListAsync();
        }
        public async Task<DashboardData> GetDashboardDataAsync()
        {
            // Fetch the counts asynchronously
            int drivers = await _context.ApplicationUser.CountAsync(a => a.Roles == "Driver");
            int maintenances = await _context.Maintenance.CountAsync();
            int buses = await _context.Bus.CountAsync();
            int routes = await _context.Routing.CountAsync();
            int announcments = await _context.Announcement.CountAsync();
            return new DashboardData
            {
                Drivers = drivers,
                Maintenances = maintenances,
                Buses = buses,
                Routes = routes,
                Announcement = announcments
            };
        }

        public async Task<List<ApplicationUser>> GetAllDrivers()
        {
            try
            {
                var allUSers = await _context.Users.Where(a => a.UserType == "Driver").ToListAsync();
                return allUSers;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<Maintenance>> GetAllMaintenanceRequests()
        {
            try
            {
                var allRequests = await _context.Maintenance.ToListAsync();
                return allRequests;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<Maintenance> ApprovedMaintenanceRequest(string ifApprove, MaintenancaDTO maintenanceDTO)
        {
            var maintenance = await _context.Maintenance.Where(e => e.BusId == maintenanceDTO.BusId).FirstOrDefaultAsync();
            var bus = await _context.DriverProfile.Where(e => e.BusId == maintenanceDTO.BusId).FirstOrDefaultAsync();
            var driver = await _userManager.FindByIdAsync(bus.DriverId);
            if (ifApprove == "Yes")
            {
                maintenance.Status = "Approved";
                bus.Status = "Under Maintenance";
                _context.Entry(maintenance).State = EntityState.Modified;
                _context.Entry(bus).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                SendOtpViaEmail("Your Request For The Maintenance Has Been Approved", driver.Email, "Maintenance Approval");
            }
            return maintenance;
        }
        public async Task<UserDTO> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.ApplicationUser.Where(a => a.Email == email).FirstOrDefaultAsync();
                var userDTO = new UserDTO
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserName = user.UserName                    
                };
                return userDTO;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<ApplicationUser> CreateUser(SignUpDTO signUpDto)
        {
            var phone = int.Parse(signUpDto.Phone);
            var addedUser = new ApplicationUser
            {
                UserName = signUpDto.UserName,
                Email = signUpDto.UserEmail,
                UserType = "Driver",
                PhoneNumber = phone.ToString(),
                Roles = "Driver"
            };
            var resultWithPass = await _userManager.CreateAsync(addedUser, signUpDto.Password);
            await _userManager.AddToRolesAsync(addedUser, signUpDto.Roles);
            await _context.SaveChangesAsync();
            return addedUser;
        }
        public async Task<Announcement> CreateAnnouncement(string email, AnnouncementDTO AnnouncementDto)
        {
            var addedAnnouncement = new Announcement
            {
                AdminId = AnnouncementDto.AdminId,
                Audience = AnnouncementDto.Audience,
                Title = AnnouncementDto.Title,
                Content = AnnouncementDto.Content,
                CreatedTime = DateTime.Now
            };
            _context.Announcement.Add(addedAnnouncement);
            await _context.SaveChangesAsync();
            return addedAnnouncement;
        }
        public async Task<bool> DeleteUser(string email)
        {
            // Find the user by email
            var user = await _context.ApplicationUser
                                      .Where(a => a.Email == email)
                                      .FirstOrDefaultAsync();
            if (user == null)
            {
                return false; // User not found
            }
            // Remove the user and save changes
            _context.ApplicationUser.Remove(user);
            await _context.SaveChangesAsync();
            return true; // Successfully deleted
        }
        void SendOtpViaEmail(string mess, string email, string subject)
        {
            MailMessage message = new MailMessage();
            message.Subject = subject;
            message.Body = mess;
            message.From = new MailAddress("ayawahidi@outlook.com", "Admin");
            message.To.Add(new MailAddress(email, "Recipient 1"));
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.office365.com";
            client.Credentials = new NetworkCredential("ayawahidi@outlook.com", "Roro2001**");
            client.Port = 587;
            client.EnableSsl = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
        public async Task<Routing> GetRoutingById(string routingId)
        {
            try
            {
                return await _context.Routing.Include(r => r.StopPoints)
                    .FirstOrDefaultAsync(r => r.RoutingId == routingId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error retrieving routing by ID: {e.Message}");
                throw;
            }
        }
        public async Task<Routing> CreateRouting(RoutingDTO routingDto)
        {
            var newRouting = new Routing
            {
                RouteName = routingDto.RouteName,
                StartPoint = routingDto.StartPoint,
                EndPoint = routingDto.EndPoint,
                TotalDistance = routingDto.TotalDistance,
                EstimatedTime = TimeSpan.Parse(routingDto.EstimatedTime.ToString()) // Ensure correct parsing
            };
            // Add each stop point to the routing
            foreach (var stopDto in routingDto.StopPoints)
            {
                var stopPoint = new StopPoint
                {
                    Name = stopDto.Name,
                    EstimatedTime = TimeSpan.Parse(stopDto.EstimatedTime.ToString()), // Ensure correct parsing
                    RoutingId = newRouting.RoutingId // This should not be manually set
                };
                newRouting.StopPoints.Add(stopPoint);
            }
            _context.Routing.Add(newRouting);
            await _context.SaveChangesAsync();
            return newRouting;
        }
        public async Task<Schedule> CreateSchedule(ScheduleDTO scheduleDto)
        {
            var newSchedule = new Schedule
            {
                DriverId = scheduleDto.DriverId,
                RoutingId = scheduleDto.RoutingId,
                StartTime = scheduleDto.StartTime,
                EndTime = scheduleDto.EndTime,
                EstimatedTime = scheduleDto.EstimatedTime,
                Status = scheduleDto.Status
            };
            _context.Schedule.Add(newSchedule);
            await _context.SaveChangesAsync();
            return newSchedule;
        }
        
    }
}