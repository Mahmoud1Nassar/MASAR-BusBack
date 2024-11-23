using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MASAR.Model;

namespace MASAR.Repositories.Services
{
    public class JwtTokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public JwtTokenService(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public static TokenValidationParameters ValidateToken(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(configuration),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }

        private static SecurityKey GetSecurityKey(IConfiguration configuration)
        {  
            var secretKey = configuration["JWT:SecretKey"];
            if (secretKey == null)
            {
                throw new InvalidOperationException("Jwt Secret key is not exsist");
            }
            var secretBytes = Encoding.UTF8.GetBytes(secretKey);

            return new SymmetricSecurityKey(secretBytes);
        }

        public async Task<string> GenerateToken(ApplicationUser applicationUser, TimeSpan expiryDate)
        {
            var userPrincliple = await _signInManager.CreateUserPrincipalAsync(applicationUser);
            if (userPrincliple == null)
            {
                return null;
            }
            var claims = userPrincliple.Claims.ToList();
            var userPesmissions = await _userManager.GetClaimsAsync(applicationUser);

            //Add the marge to the end of the token
            claims.AddRange(userPesmissions);

            var signInKey = GetSecurityKey(_configuration);

            var token = new JwtSecurityToken
                (
                expires: DateTime.UtcNow + expiryDate,
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256),
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}