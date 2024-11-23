using MASAR.Data;
using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MASAR.Repositories.Services
{
    public class IdentityAccountService : IUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MASARDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenService _jwtTokenService;
        // Static variables to maintain state
        private static int? ran;
        private static string? email;
        public IdentityAccountService(UserManager<ApplicationUser> userManager, IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager, JwtTokenService jwtTokenService, MASARDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
            _configuration = configuration;
        }
      
        public async Task<UserDTO> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return null; // Handle invalid login attempt
            }

            bool isProfileCreated = _context.DriverProfile.Any(dp => dp.DriverId == user.Id);

            return new UserDTO()
            {
                UserId = user.Id,
                UserEmail = user.Email,
                Token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60)),
                IsProfileCreated = isProfileCreated
            };
        }

        public async Task<UserDTO> Logout(string email)
        {
            var logoutAccount = await _userManager.FindByEmailAsync(email);
            if (logoutAccount == null)
            {
                return null; // Handle user not found case
            }

            await _signInManager.SignOutAsync();
            return new UserDTO()
            {
                UserId = logoutAccount.Id,
                UserEmail = logoutAccount.Email
            };
        }

        public async Task ResetPassword(string email1)
        {
            var resetEmail = await _userManager.FindByEmailAsync(email1);
            if (resetEmail != null)
            {
                ran = new Random().Next(1111, 9999); // Generate random code
                MailjetService service = new MailjetService(_configuration);
                await service.SendEmailAsync(resetEmail.Email, ran.ToString());
                email = email1;
            }
        }
        public  bool ValidateCode(int code)
        {
            return ran.HasValue && code == ran ; // Validate the code
        }
        public async Task<string> NewPassword(string newPassword, int c)
        {
            bool code = ValidateCode(c);
            if (code)
            {
                // Find the user by the stored email
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    // Set the new password directly
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);
                    // Update the user in the database
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return "Password has been reset successfully.";
                    }
                    return "Failed to reset password.";
                }
            }
            return "User not found.";
        }
        public async Task<IdentityResult> ChangePassword(string newPassword, string oldPassword, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result;
        }
        public async Task<UserDTO> GetToken(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new InvalidOperationException("Token Is Not Exist!");
            }
            return new UserDTO()
            {
                UserEmail = user.Email,
                Token = await _jwtTokenService.GenerateToken(user, TimeSpan.FromMinutes(60))
            };
        }
        public async Task<List<AnnouncementDTO>> ViewAnnouncements()
        {
            return await _context.Announcement
                .Where(a => a.Audience == "All")
                .Select(b => new AnnouncementDTO
                {
                    Title = b.Title,
                    Content = b.Content,
                    CreatedTime = b.CreatedTime
                })
                .ToListAsync();
        }
        public async Task<ApplicationUser> UpdateUser(string email, ApplicationUser user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return user;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
