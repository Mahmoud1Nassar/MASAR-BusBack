using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MASAR.Data;
using MASAR.Model;
using MASAR.Repositories.Interfaces;
using MASAR.Model.DTO;
using Microsoft.AspNetCore.Authorization;
namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudent _context;
        public StudentController(IStudent context)
        {
            _context = context;
        }
        //GET : api/Student
        [HttpGet("ViewAllAnnouncement")]
        [Authorize(Policy = "RequireStudentRole" )]
        public async Task<IActionResult> GetAllAnnouncements()
        {
            var announcements = await _context.ViewAnnouncements();
            return Ok(announcements);
        }      
        //// GET: api/Users/StudentRigester
        [Route("StudentRigester")]
        [HttpPost]
        // for creating Student
        public async Task<ActionResult<UserDTO>> StudentRigester(SignUpDTO user)
        {
            return await _context.Register(user);
        }

        [HttpGet("getAllRoutes")]
        public async Task<ActionResult<IEnumerable<Routing>>> GetAllRoutes()
        {
            var routes = await _context.GetAllRoutes();
            return Ok(routes);
        }

        [HttpGet("SearchRouteByName")]
        public async Task<IActionResult> SearchRouteByName(string routeName)
        {
                var routes = await _context.SearchRouteByName(routeName);
                if (routes == null)
                {
                    return NotFound("No routes found matching the search criteria.");
                }
                return Ok(routes);
        }

        [HttpGet("getAllSchedules")]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetAllSchedules()
        {
            var schedules = await _context.GetAllSchedules();
            return Ok(schedules);
        }

        // GET: api/Student/getAllStopPoints
        [HttpGet("getAllStopPoints")]
        public async Task<ActionResult<IEnumerable<StopPoint>>> GetAllStopPoints()
        {
            var stopPoints = await _context.GetAllStopPoints();
            if (stopPoints == null || stopPoints.Count == 0)
            {
                return NotFound();
            }
            return Ok(stopPoints);
        }

        [Route("EditStudentProfile")]
        [HttpPost]
        [Authorize(Policy = "RequireAdminStudentRole")]
        // for creating Student
        public async Task<ActionResult<EditUserDTO>> EditStudentProfile(string email, EditUserDTO user)
        {
            return await _context.EditUserProfile(email, user);
        }
        [Route("GetStudentById")]
        [HttpGet]
        [Authorize(Policy = "RequireAdminStudentRole")]
        public async Task<ActionResult<ApplicationUser>> GetStudentById(string Id)
        {
            return await _context.GetUserById(Id);
        }
        //Get: 
        [HttpGet("GetStudentRoutById")]
        public async Task<ActionResult<List<string>>> GetAllFavioretRoutForUser(string userId)
        {
            return await _context.GetAllFavioretRoutForUser(userId);
        }
        // POST: api/FavoritRout
        [Route("AddFavRoutForUser")]
        [HttpPost]
        public async Task<ActionResult<FavoriteRoute>> AddFavRoutForUser(string userId, string routId)
        {
            return await _context.AddRoutForUser(userId, routId);
        }
        [HttpDelete("RemoveFavRoutForUser")]
        //for deleting driver
        public async Task<ActionResult<FavoriteRoute>> RemoveFavRoutForUser(string userId, string routId)
        {
            return await _context.RemoveRoutForUser(userId, routId);
        }
    }
}