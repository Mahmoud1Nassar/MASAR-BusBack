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

            var routeAId = Guid.NewGuid().ToString();
            var routeBId = Guid.NewGuid().ToString();
            var routeCId = Guid.NewGuid().ToString();
            // Seed data
            modelBuilder.Entity<Routing>().HasData(new List<Routing>
            {
                new Routing
                {
                    RoutingId = routeAId,
                    RouteName = "Khalda",
                    StartPoint = "LTUC",
                    EndPoint = "Khalda",
                    TotalDistance = 15.5,
                    EstimatedTime = TimeSpan.FromMinutes(60)
                },
                new Routing
                {
                    RoutingId = routeBId,
                    RouteName = "Al-Shmesaani",
                    StartPoint = "LTUC",
                    EndPoint = "Al-Shmesaani",
                    TotalDistance = 20.3,
                    EstimatedTime = TimeSpan.FromMinutes(60)
                },
                new Routing
                {
                    RoutingId = routeCId,
                    RouteName = "Al-Ashrafiyah",
                    StartPoint = "LTUC",
                    EndPoint = "Al-Ashrafiyah",
                    TotalDistance = 25.0,
                    EstimatedTime = TimeSpan.FromMinutes(60)
                }
            });

            // Seed StopPoints
            modelBuilder.Entity<StopPoint>().HasData(new List<StopPoint>
        {
            new StopPoint { StopPointId = 1, Name = "Airport St", EstimatedTime = TimeSpan.FromMinutes(20), RoutingId = routeAId },
            new StopPoint { StopPointId = 2, Name = "7th Circle", EstimatedTime = TimeSpan.FromMinutes(25), RoutingId = routeAId },
            new StopPoint { StopPointId = 3, Name = "Mecca St", EstimatedTime = TimeSpan.FromMinutes(15), RoutingId = routeAId },
            new StopPoint { StopPointId = 4, Name = "Khalda", EstimatedTime = TimeSpan.FromMinutes(10), RoutingId = routeAId },
            new StopPoint { StopPointId = 5, Name = "Airport St", EstimatedTime = TimeSpan.FromMinutes(12), RoutingId = routeBId },
            new StopPoint { StopPointId = 6, Name = "Abdoun", EstimatedTime = TimeSpan.FromMinutes(16), RoutingId = routeBId },
            new StopPoint { StopPointId = 7, Name = "Um Uthaina", EstimatedTime = TimeSpan.FromMinutes(20), RoutingId = routeBId },
            new StopPoint { StopPointId = 8, Name = "Al-Rabiya", EstimatedTime = TimeSpan.FromMinutes(20), RoutingId = routeBId },
            new StopPoint { StopPointId = 9, Name = "Al-Shmesaani", EstimatedTime = TimeSpan.FromMinutes(20), RoutingId = routeBId },
            new StopPoint { StopPointId = 10, Name = "Al-Quds St", EstimatedTime = TimeSpan.FromMinutes(25), RoutingId = routeCId },
            new StopPoint { StopPointId = 11, Name = "Al-Muqabalain", EstimatedTime = TimeSpan.FromMinutes(15), RoutingId = routeCId },
            new StopPoint { StopPointId = 12, Name = "Hai Nazaal", EstimatedTime = TimeSpan.FromMinutes(15), RoutingId = routeCId },
            new StopPoint { StopPointId = 13, Name = "Al-Ashrafiyah", EstimatedTime = TimeSpan.FromMinutes(20), RoutingId = routeCId }
        });
            ///You should have 3 Drivers before seed these data to the DB
            //modelBuilder.Entity<Schedule>().HasData(new List<Schedule>
            //{
            //    new Schedule
            //    {
            //        ScheduleId = "1",
            //        DriverId = "8a9b187b-ec1f-4d41-a158-8a39524f040d",
            //        RoutingId = "d3c0acee-71a4-4d84-91d9-fa81b2ed68fc",
            //        StartTime = DateTime.Today.AddHours(8),
            //        EndTime = DateTime.Today.AddHours(9),
            //        EstimatedTime = DateTime.Now.AddMinutes(60),
            //        Status = "Scheduled"
            //    },
            //     new Schedule
            //    {
            //        ScheduleId = "2",
            //        DriverId = "8a9b187b-ec1f-4d41-a158-8a39524f040d",
            //        RoutingId = "d3c0acee-71a4-4d84-91d9-fa81b2ed68fc",
            //        StartTime = DateTime.Today.AddHours(12),
            //        EndTime = DateTime.Today.AddHours(13),
            //        EstimatedTime = DateTime.Now.AddMinutes(60),
            //        Status = "Scheduled"
            //    },
            //      new Schedule
            //    {
            //        ScheduleId = "3",
            //        DriverId = "8a9b187b-ec1f-4d41-a158-8a39524f040d",
            //        RoutingId = "d3c0acee-71a4-4d84-91d9-fa81b2ed68fc",
            //        StartTime = DateTime.Today.AddHours(16),
            //        EndTime = DateTime.Today.AddHours(17),
            //        EstimatedTime = DateTime.Now.AddMinutes(90),
            //        Status = "Scheduled"
            //    },
            //    new Schedule
            //    {
            //        ScheduleId = "4",
            //        DriverId = "831fc18c-2c96-48c3-9ea2-11c69d41338e",
            //        RoutingId = "c8200991-9fa3-43d9-817f-ea03f6954581",
            //        StartTime = DateTime.Today.AddHours(8),
            //        EndTime = DateTime.Today.AddHours(9),
            //        EstimatedTime = DateTime.Now.AddMinutes(80),
            //        Status = "In Progress"
            //    },
            //       new Schedule
            //    {
            //        ScheduleId = "5",
            //        DriverId = "831fc18c-2c96-48c3-9ea2-11c69d41338e",
            //        RoutingId = "c8200991-9fa3-43d9-817f-ea03f6954581",
            //        StartTime = DateTime.Today.AddHours(12),
            //        EndTime = DateTime.Today.AddHours(13),
            //        EstimatedTime = DateTime.Now.AddMinutes(60),
            //        Status = "In Progress"
            //    },
            //          new Schedule
            //    {
            //        ScheduleId = "6",
            //        DriverId = "831fc18c-2c96-48c3-9ea2-11c69d41338e",
            //        RoutingId = "c8200991-9fa3-43d9-817f-ea03f6954581",
            //        StartTime = DateTime.Today.AddHours(16),
            //        EndTime = DateTime.Today.AddHours(17),
            //        EstimatedTime = DateTime.Now.AddMinutes(60),
            //        Status = "In Progress"
            //    },
            //    new Schedule
            //    {
            //        ScheduleId = "7",
            //        DriverId = "56d5a80e-d1c4-490e-ab72-d7856738565e",
            //        RoutingId = "e0ae1106-4ad1-433d-b6f7-82d239fd12be",
            //        StartTime = DateTime.Today.AddHours(8),
            //        EndTime = DateTime.Today.AddHours(9),
            //        EstimatedTime = DateTime.Now.AddMinutes(80),
            //        Status = "Scheduled"
            //    },
            //      new Schedule
            //    {
            //        ScheduleId = "8",
            //        DriverId = "56d5a80e-d1c4-490e-ab72-d7856738565e",
            //        RoutingId = "e0ae1106-4ad1-433d-b6f7-82d239fd12be",
            //        StartTime = DateTime.Today.AddHours(12),
            //        EndTime = DateTime.Today.AddHours(13),
            //        EstimatedTime = DateTime.Now.AddMinutes(60),
            //        Status = "Scheduled"
            //    },
            //       new Schedule
            //    {
            //        ScheduleId = "9",
            //        DriverId = "56d5a80e-d1c4-490e-ab72-d7856738565e",
            //        RoutingId = "e0ae1106-4ad1-433d-b6f7-82d239fd12be",
            //        StartTime = DateTime.Today.AddHours(16),
            //        EndTime = DateTime.Today.AddHours(17),
            //        EstimatedTime = DateTime.Now.AddMinutes(90),
            //        Status = "Scheduled"
            //    }
            //});
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