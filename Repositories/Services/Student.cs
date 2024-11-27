using MASAR.Model.DTO;
using MASAR.Model;
using Microsoft.AspNetCore.Identity;
using MASAR.Data;
using MASAR.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Mailjet.Client.Resources;
namespace MASAR.Repositories.Services
{
    public class Student : IStudent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MASARDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // inject jwt service
        private JwtTokenService _jwtTokenService;
        public Student(UserManager<ApplicationUser> Manager,
            SignInManager<ApplicationUser> signInManager, JwtTokenService jwtTokenService, MASARDBContext context)
        {
            _userManager = Manager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        
        public async Task<List<AnnouncementDTO>> ViewAnnouncements()
        {
            var allAnnoun = await _context.Announcement
                .Where(a => a.Audience == "Student")
                .Select(a => new AnnouncementDTO
                {
                    Title = a.Title,
                    Content = a.Content,
                    CreatedTime = a.CreatedTime
                })
                .ToListAsync();

            return allAnnoun;
        }
        public async Task<UserDTO> Register(SignUpDTO signUpDTO)
        {
            var user = new ApplicationUser()
            {
                UserName = signUpDTO.UserName,
                Email = signUpDTO.UserEmail,
                UserType = "Student",
                PhoneNumber = signUpDTO.Phone,
                Roles = "Student"
            };
            //var roles = new List<string> { "Student" };
            //signUpDTO.Roles = roles;
            var resultWithPass = await _userManager.CreateAsync(user, signUpDTO.Password);
            await _userManager.AddToRolesAsync(user, signUpDTO.Roles);
            await _context.SaveChangesAsync();
            var result = new UserDTO()
            {
                UserId = user.Id,
                UserEmail = user.Email
            };
            return result;
        }

        public async Task<List<Routing>> GetAllRoutes()
        {
            try
            {
                return await _context.Routing
                                     .Include(r => r.StopPoints)
                                     .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error retrieving all routes: {e.Message}");
                return new List<Routing>();
            }
        }

        public async Task<List<Routing>> SearchRouteByName(string routeName)
        {
            return await _context.Routing 
            .Where(sp => (string.IsNullOrEmpty(routeName) || sp.RouteName.Contains(routeName))).ToListAsync();
        }

        public async Task<List<ScheduleDTO>> GetAllSchedules()
        {
            try
            {
                return await _context.Schedule
                    .Select(s => new ScheduleDTO
                    {
                        DriverId = s.DriverId,
                        RoutingId = s.RoutingId,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        EstimatedTime = s.EstimatedTime,
                        Status = s.Status
                    })
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error retrieving all schedules: {e.Message}");
                return new List<ScheduleDTO>();
            }
        }

        public async Task<List<StopPoint>> GetAllStopPoints()
        {
            try
            {
                return await _context.StopPoints.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error retrieving stop points: {e.Message}");
                return new List<StopPoint>();
            }
        }

        public async Task<EditUserDTO> EditUserProfile(string email, EditUserDTO edit)
        {
            try
            {
                var studentByEmail = await _userManager.FindByEmailAsync(email);
                if (studentByEmail != null && email == edit.userEmail)
                {
                    studentByEmail.PhoneNumber = edit.userPhone;
                    if (!string.IsNullOrEmpty(edit.NewPassword))
                    {
                        var passwordResult = await _userManager.ChangePasswordAsync(studentByEmail, edit.OldPassword, edit.NewPassword);
                    }
                    _context.Entry(studentByEmail).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return edit;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
                return null;
            }
        }
        public Task<ApplicationUser> GetUserById(string Id)
        {
            var studentById = _userManager.FindByIdAsync(Id);
            return studentById;
        }

        public async Task<List<string>> GetAllFavioretRoutForUser(string UserId)
        {
            var routsForUser = await _context.FavoriteRoute.Where(a => a.StudentId == UserId).Select(a => a.RoutingId).ToListAsync();
            return routsForUser;
        }
        public async Task<FavoriteRoute> AddRoutForUser(string StudentId, string RoutId)
        {
            // Check if the combination already exists
            var existingFavorite = await _context.FavoriteRoute
                .FirstOrDefaultAsync(fr => fr.StudentId == StudentId && fr.RoutingId == RoutId);
            if (existingFavorite != null)
            {
                return null;
            }
            var AddFavRout = new FavoriteRoute
            {
                RoutingId = RoutId,
                StudentId = StudentId
            };
            _context.FavoriteRoute.Add(AddFavRout);
            await _context.SaveChangesAsync();
            return AddFavRout;
        }
        public async Task<FavoriteRoute> RemoveRoutForUser(string StudentId, string RoutId)
        {
            var existingFavorite = await _context.FavoriteRoute
               .FirstOrDefaultAsync(fr => fr.StudentId == StudentId && fr.RoutingId == RoutId);
            if (existingFavorite != null)
            {
                _context.FavoriteRoute.Remove(existingFavorite);
                await _context.SaveChangesAsync();
                return existingFavorite;
            }
            return null;
        }
    }
}