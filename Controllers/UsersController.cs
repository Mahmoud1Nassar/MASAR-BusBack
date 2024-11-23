using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _context;
        public UsersController(IUser context)
        {
            _context = context;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(string email, string password)
        {
            var newLogin = await _context.Login(email, password);
            if (newLogin == null)
            {
                return Unauthorized(); // Return 401 if login fails
            }
            return newLogin;
        }
        [HttpPost("Logout")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<UserDTO>> Logout(string email)
        {
            var newLogout = await _context.Logout(email);
            return newLogout; // Return 404 if logout user not found
        }
        [HttpGet("ViewAllAnnouncement")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<List<AnnouncementDTO>> ViewAllAnnouncement()
        {
            return await _context.ViewAnnouncements();
        }
        [HttpPut("updateUserByEmail/{email}")]
        [Authorize(Policy = "RequireAdminStudentRole")]
        public async Task<ActionResult<ApplicationUser>> PutUser(string email, ApplicationUser user)
        {
            var updatedUser = await _context.UpdateUser(email, user);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return updatedUser;
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                await _context.ResetPassword(email);
                return Ok("Verification code sent to email.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ValidateCode")]
        public async Task<IActionResult> ValidateCode(int code)
        {
            try
            {
                var isValid =  _context.ValidateCode(code);
                if (!isValid)
                {
                    return BadRequest("Invalid or expired code.");
                }
                return Ok("Code is valid.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("NewPassword")]
        public async Task<IActionResult> NewPassword(string newPassword,int code)
        {
            try
            {
                var result = await _context.NewPassword(newPassword,code );
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ChangePassword")]
        [Authorize(Policy = "RequireAdminDriverRole")]
        public async Task<IActionResult> ChangePassword(string newPassword, string oldPassword,string userId)
        {
            try
            {
                var result = await _context.ChangePassword(newPassword, oldPassword,userId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}