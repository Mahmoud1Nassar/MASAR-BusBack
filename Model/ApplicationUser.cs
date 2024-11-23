using Microsoft.AspNetCore.Identity;

namespace MASAR.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; }
        public string Roles { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
        public ICollection<FavoriteRoute> FavoriteRoute { get; set; }
        public DriverProfile DriverProfiles { get; set; }
    }
}
