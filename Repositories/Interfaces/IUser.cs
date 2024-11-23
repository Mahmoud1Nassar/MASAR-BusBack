using MASAR.Model;
using MASAR.Model.DTO;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MASAR.Repositories.Interfaces
{
    public interface IUser
    {
        Task<UserDTO> Login(string username, string password);
        Task<UserDTO> Logout(string username);
        Task<UserDTO> GetToken(ClaimsPrincipal claimsPrincipal);
        Task<ApplicationUser> UpdateUser(string email, ApplicationUser user);
        Task<List<AnnouncementDTO>> ViewAnnouncements();
        // For student Only {
        // New methods for password reset functionality
        Task ResetPassword(string email);
        bool ValidateCode(int code); // Return true if valid, false otherwise
        Task<string> NewPassword(string newPassword , int code); // Return true if the password is changed successfully
        // }
        // For Driver And Admin {
        Task<IdentityResult> ChangePassword(string newPassword , string oldPassword , string userId);
        // }
    }
}
