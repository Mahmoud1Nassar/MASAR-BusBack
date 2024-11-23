using System.ComponentModel.DataAnnotations;

namespace MASAR.Model.DTO
{
    public class SignUpDTO
    {
        public string UserType { get; set; }        
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        [Required]
        public IList<string> Roles { get; set; } // Admin - Driver - Student
    }
}