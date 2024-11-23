using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MASAR.Migrations
{
    /// <inheritdoc />
    public partial class dbMasar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Roles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bus",
                columns: table => new
                {
                    BusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlateNumber = table.Column<int>(type: "int", nullable: false),
                    LicenseExpiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CapacityNumber = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bus", x => x.BusId);
                });

            migrationBuilder.CreateTable(
                name: "Routing",
                columns: table => new
                {
                    RoutingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RouteName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDistance = table.Column<double>(type: "float", nullable: false),
                    EstimatedTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routing", x => x.RoutingId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcement",
                columns: table => new
                {
                    AnnouncementId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdminId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Audience = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcement", x => x.AnnouncementId);
                    table.ForeignKey(
                        name: "FK_Announcement_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusLocation",
                columns: table => new
                {
                    BusLocationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    PreviuseLatitude = table.Column<double>(type: "float", nullable: true),
                    PreviuseLongitude = table.Column<double>(type: "float", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusLocation", x => x.BusLocationId);
                    table.ForeignKey(
                        name: "FK_BusLocation_Bus_BusId",
                        column: x => x.BusId,
                        principalTable: "Bus",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverProfile",
                columns: table => new
                {
                    DriverProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LicenseNumber = table.Column<int>(type: "int", nullable: false),
                    BusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverProfile", x => x.DriverProfileId);
                    table.ForeignKey(
                        name: "FK_DriverProfile_AspNetUsers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverProfile_Bus_BusId",
                        column: x => x.BusId,
                        principalTable: "Bus",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteRoute",
                columns: table => new
                {
                    FavoriteRouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoutingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteRoute", x => x.FavoriteRouteId);
                    table.ForeignKey(
                        name: "FK_FavoriteRoute_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteRoute_Routing_RoutingId",
                        column: x => x.RoutingId,
                        principalTable: "Routing",
                        principalColumn: "RoutingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    ScheduleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoutingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_Schedule_AspNetUsers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schedule_Routing_RoutingId",
                        column: x => x.RoutingId,
                        principalTable: "Routing",
                        principalColumn: "RoutingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StopPoints",
                columns: table => new
                {
                    StopPointId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    RoutingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopPoints", x => x.StopPointId);
                    table.ForeignKey(
                        name: "FK_StopPoints_Routing_RoutingId",
                        column: x => x.RoutingId,
                        principalTable: "Routing",
                        principalColumn: "RoutingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Maintenance",
                columns: table => new
                {
                    MaintenanceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpectedMaintenanceDays = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenance", x => x.MaintenanceId);
                    table.ForeignKey(
                        name: "FK_Maintenance_Bus_BusId",
                        column: x => x.BusId,
                        principalTable: "Bus",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintenance_DriverProfile_DriverId",
                        column: x => x.DriverId,
                        principalTable: "DriverProfile",
                        principalColumn: "DriverProfileId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin", "483ea611-f1ba-4f34-98c3-7a2302dd52c0", "Admin", "ADMIN" },
                    { "driver", "5cf38f86-70a7-490a-8cb4-c9a1812ddb0b", "Driver", "DRIVER" },
                    { "student", "ec037015-af0f-4f23-a2cf-ba03044bf22a", "Student", "STUDENT" },
                    { "user", "5d3ea3ef-e8eb-492f-8424-112606137e47", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Routing",
                columns: new[] { "RoutingId", "EndPoint", "EstimatedTime", "RouteName", "StartPoint", "TotalDistance" },
                values: new object[,]
                {
                    { "194cfe42-e751-4c4e-aff3-5efc30260035", "Al-Ashrafiyah", new TimeSpan(0, 1, 0, 0, 0), "Al-Ashrafiyah", "LTUC", 25.0 },
                    { "2a26bc3b-51da-4821-9546-0f81b53a9eb2", "Al-Shmesaani", new TimeSpan(0, 1, 0, 0, 0), "Al-Shmesaani", "LTUC", 20.300000000000001 },
                    { "4ccdea06-f510-4023-ab3f-cc48b8dad810", "Khalda", new TimeSpan(0, 1, 0, 0, 0), "Khalda", "LTUC", 15.5 }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { -1742857321, "permission", "update", "driver" },
                    { -1284749112, "permission", "update", "user" },
                    { -967882180, "permission", "read", "user" },
                    { -551999624, "permission", "create", "admin" },
                    { -408381390, "permission", "update", "admin" },
                    { 794019726, "permission", "update", "student" },
                    { 1041763754, "permission", "read", "admin" },
                    { 1080902927, "permission", "read", "driver" },
                    { 1362574069, "permission", "delete", "admin" },
                    { 1708460071, "permission", "read", "student" }
                });

            migrationBuilder.InsertData(
                table: "StopPoints",
                columns: new[] { "StopPointId", "EstimatedTime", "Name", "RoutingId" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 20, 0, 0), "Airport St", "4ccdea06-f510-4023-ab3f-cc48b8dad810" },
                    { 2, new TimeSpan(0, 0, 25, 0, 0), "7th Circle", "4ccdea06-f510-4023-ab3f-cc48b8dad810" },
                    { 3, new TimeSpan(0, 0, 15, 0, 0), "Mecca St", "4ccdea06-f510-4023-ab3f-cc48b8dad810" },
                    { 4, new TimeSpan(0, 0, 10, 0, 0), "Khalda", "4ccdea06-f510-4023-ab3f-cc48b8dad810" },
                    { 5, new TimeSpan(0, 0, 12, 0, 0), "Airport St", "2a26bc3b-51da-4821-9546-0f81b53a9eb2" },
                    { 6, new TimeSpan(0, 0, 16, 0, 0), "Abdoun", "2a26bc3b-51da-4821-9546-0f81b53a9eb2" },
                    { 7, new TimeSpan(0, 0, 20, 0, 0), "Um Uthaina", "2a26bc3b-51da-4821-9546-0f81b53a9eb2" },
                    { 8, new TimeSpan(0, 0, 20, 0, 0), "Al-Rabiya", "2a26bc3b-51da-4821-9546-0f81b53a9eb2" },
                    { 9, new TimeSpan(0, 0, 20, 0, 0), "Al-Shmesaani", "2a26bc3b-51da-4821-9546-0f81b53a9eb2" },
                    { 10, new TimeSpan(0, 0, 25, 0, 0), "Al-Quds St", "194cfe42-e751-4c4e-aff3-5efc30260035" },
                    { 11, new TimeSpan(0, 0, 15, 0, 0), "Al-Muqabalain", "194cfe42-e751-4c4e-aff3-5efc30260035" },
                    { 12, new TimeSpan(0, 0, 15, 0, 0), "Hai Nazaal", "194cfe42-e751-4c4e-aff3-5efc30260035" },
                    { 13, new TimeSpan(0, 0, 20, 0, 0), "Al-Ashrafiyah", "194cfe42-e751-4c4e-aff3-5efc30260035" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_AdminId",
                table: "Announcement",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BusLocation_BusId",
                table: "BusLocation",
                column: "BusId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverProfile_BusId",
                table: "DriverProfile",
                column: "BusId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverProfile_DriverId",
                table: "DriverProfile",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteRoute_RoutingId",
                table: "FavoriteRoute",
                column: "RoutingId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteRoute_StudentId",
                table: "FavoriteRoute",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_BusId",
                table: "Maintenance",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_DriverId",
                table: "Maintenance",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_DriverId",
                table: "Schedule",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_RoutingId",
                table: "Schedule",
                column: "RoutingId");

            migrationBuilder.CreateIndex(
                name: "IX_StopPoints_RoutingId",
                table: "StopPoints",
                column: "RoutingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcement");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BusLocation");

            migrationBuilder.DropTable(
                name: "FavoriteRoute");

            migrationBuilder.DropTable(
                name: "Maintenance");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "StopPoints");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DriverProfile");

            migrationBuilder.DropTable(
                name: "Routing");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Bus");
        }
    }
}
