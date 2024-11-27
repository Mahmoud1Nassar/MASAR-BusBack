using MASAR.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace MASAR.Data
{
    public class MASARDBContext : IdentityDbContext<ApplicationUser>
    {
        public MASARDBContext(DbContextOptions<MASARDBContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<BusLocation> BusLocation { get; set; }
        public DbSet<Maintenance> Maintenance { get; set; }
        public DbSet<DriverProfile> DriverProfile { get; set; }
        public DbSet<Announcement> Announcement { get; set; }
        public DbSet<Bus> Bus { get; set; }
        public DbSet<Routing> Routing { get; set; }
        public DbSet<StopPoint> StopPoints { get; set; }
        public DbSet<FavoriteRoute> FavoriteRoute { get; set; }

        //ERD Visualization
        //The entities will be linked as follows:
        //User(1) — (M)Schedule
        //User(1) — (M)Announcement
        //Schedule(M) — (1) Route
        //Maintenance(M) — (1) DriverProfile
        //Maintenance(M) — (1) Bus
        //DriverProfile(1) — (1) Bus
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // User -> Schedules
            modelBuilder.Entity<ApplicationUser>()
              .HasMany(u => u.Schedules)
              .WithOne(s => s.User)
            .HasForeignKey(s => s.DriverId) // Correct foreign key
              .OnDelete(DeleteBehavior.Restrict);
            // User -> Announcements
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Announcements)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
            // Schedule -> Route
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Routing)
                .WithMany(r => r.Schedules)
                .HasForeignKey(s => s.RoutingId)
                .OnDelete(DeleteBehavior.Restrict);
            // Maintenance -> DriverProfile
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.DriverProfile)
                .WithMany(b => b.Maintenances)
                .HasForeignKey(m => m.DriverId)
                .OnDelete(DeleteBehavior.Restrict);
            // Maintenance -> Bus
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Bus)
                .WithMany(b => b.Maintenances)
                .HasForeignKey(m => m.BusId)
                .OnDelete(DeleteBehavior.Restrict);
            // DriverProfile -> Bus (One-to-One)

            modelBuilder.Entity<DriverProfile>()
                .HasOne(dp => dp.Bus) // Navigation property for Bus
                .WithOne(b => b.DriverProfiles) // Corresponding navigation property in Bus
                .HasForeignKey<DriverProfile>(dp => dp.BusId) // Foreign key in DriverProfile
                .OnDelete(DeleteBehavior.Restrict);
            // DriverProfile -> User (One-to-One)
            modelBuilder.Entity<DriverProfile>()
                .HasOne(dp => dp.User) // Navigation property for User
                .WithOne(u => u.DriverProfiles) // Corresponding navigation property in User
                .HasForeignKey<DriverProfile>(dp => dp.DriverId) // Foreign key in DriverProfile
                .OnDelete(DeleteBehavior.Cascade); // DriverProfile -> User (One-to-One)
            modelBuilder.Entity<BusLocation>()
                .HasOne(dp => dp.Bus) // Navigation property for User
                .WithOne(u => u.BusLocations) // Corresponding navigation property in User
                .HasForeignKey<BusLocation>(dp => dp.BusId) // Foreign key in DriverProfile
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Routing>()
                .HasMany(r => r.StopPoints)
                .WithOne(sp => sp.Routing)
                .HasForeignKey(sp => sp.RoutingId)
                .OnDelete(DeleteBehavior.Restrict);
            // FavioritRout-ApplicationUser
            modelBuilder.Entity<FavoriteRoute>()
              .HasOne(fr => fr.User)
              .WithMany(u => u.FavoriteRoute)
              .HasForeignKey(fr => fr.StudentId)
              .OnDelete(DeleteBehavior.Restrict);
            // FavioritRout-Routing
            modelBuilder.Entity<FavoriteRoute>()
                .HasOne(fr => fr.Routing)
                .WithMany(r => r.FavoriteRoute)
                .HasForeignKey(fr => fr.RoutingId)
                .OnDelete(DeleteBehavior.Restrict);     
          
            // Adding Roles 
            seedRoles(modelBuilder, "Admin", "create", "update", "delete","read");
            seedRoles(modelBuilder, "Driver", "update", "read");
            seedRoles(modelBuilder, "Student", "update", "read");
            seedRoles(modelBuilder, "User", "update" ,"read");
        }
        private void seedRoles(ModelBuilder modelBuilder, string roleName, params string[] permissions)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()  // Unique Concurrency Stamp
            };
            // Add claims for the role
            var claims = permissions.Select(permission => new IdentityRoleClaim<string>
            {
                Id = Guid.NewGuid().GetHashCode(), // Unique identifier
                RoleId = role.Id,
                ClaimType = "permission",
                ClaimValue = permission
            }).ToArray();
            // Seed the role and its claims
            modelBuilder.Entity<IdentityRole>().HasData(role);
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(claims);
        }
    }
}