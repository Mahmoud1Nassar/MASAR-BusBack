using Microsoft.AspNetCore.Mvc;
using MASAR.Model;
using MASAR.Repositories.Interfaces;
using MASAR.Model.DTO;
using Microsoft.AspNetCore.Authorization;
namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _context;
        public AdminController(IAdmin context)
        {
            _context = context;
        }
        // GET: api/Users
        [HttpGet("getAllUsers/TEST")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            return await _context.GetAllUsers();
        }
        // GET: api/Users
        [HttpGet("getAllDriversByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUser()
        {
            return await _context.GetAllDrivers();
        }
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            var data = await _context.GetDashboardDataAsync();
            return Ok(data);
        }
        //// GET: api/Users/5
        [HttpGet("getDriverByEmail/{email}")]
        public async Task<ActionResult<UserDTO>> GetUser(string email)
        {
            return await _context.GetUserByEmail(email);
        }
        //// GET: api/Users
        [HttpGet("GetAllMaintenanceRequestsByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<List<Maintenance>> GetAllMaintenanceRequests()
        {
            return await _context.GetAllMaintenanceRequests();
        }
        // PUT: api/Users
        [HttpPut("ApprovedMaintenanceRequestByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<Maintenance> ApprovedMaintenanceRequest(string ifApprove, MaintenancaDTO maintenanceDTO)
        {
            return await _context.ApprovedMaintenanceRequest(ifApprove, maintenanceDTO);
        }
        //POST: api/Users
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateDriverByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        // for adding driver
        public async Task<ActionResult<ApplicationUser>> PostUser(SignUpDTO user)
        {
            return await _context.CreateUser(user);
        }
        [HttpPost("CreateAnnouncementByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        // for adding driver
        public async Task<ActionResult<Announcement>> CreatAnnouncement(string email,AnnouncementDTO announcement)
        {
            return await _context.CreateAnnouncement(email, announcement);
        }
        //DELETE: api/Users/5
        [HttpDelete("DeleteDriverByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        //for deleting driver
        public async Task<IActionResult> DeleteDriver(string email)
        {
                await _context.DeleteUser(email); // Await the async operation
                return Ok(); // Return a 200 OK response if successful            
        }
        // POST: api/Admin/CreateRouting
        [HttpPost("CreateRouting")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateRouting([FromBody] RoutingDTO routingDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var createdRouting = await _context.CreateRouting(routingDto);
                return CreatedAtAction(nameof(GetRoutingById), new { id = createdRouting.RoutingId }, createdRouting);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // GET: api/Admin/GetRoutingById/{id}
        [HttpGet("GetRoutingById/{id}")]
        public async Task<ActionResult<Routing>> GetRoutingById(string id)
        {
            try
            {
                var routing = await _context.GetRoutingById(id);
                if (routing == null) return NotFound("Routing not found.");
                return Ok(routing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // GET: api/Admin/GetScheduleByRouteId/{id}
        [HttpGet("GetSchedulesByRouteId/{id}")]
        public async Task<ActionResult<Schedule>> GetSchedulesByRouteId(string id)
        {
            try
            {
                var schedule = await _context.GetSchedulesByRouteId(id);
                if (schedule == null) return NotFound("Schedule not found.");
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("CreateSchedule")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CreateSchedule([FromBody] ScheduleDTO scheduleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var createdSchedule = await _context.CreateSchedule(scheduleDto);
                return CreatedAtAction(nameof(CreateSchedule), new { id = createdSchedule.ScheduleId }, createdSchedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("ViewAllAnnouncementForAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<List<AnnouncementDTO>> ViewAllAnnouncement()
        {
            return await _context.ViewAnnouncementsForAdmin();
        }
    }
}