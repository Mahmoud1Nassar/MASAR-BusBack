using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MASAR.Model;
using MASAR.Repositories.Interfaces;
using MASAR.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using MASAR.Data;
using MASAR.Repositories.Services;
namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly IBus _context;
        private readonly MASARDBContext _buslocation;

        public BusesController(IBus context)
        {
            _context = context;
        }
        [HttpGet("GetAllBuses")]
        public async Task<ActionResult> GetAllBusesWithDriverName()
        {
            var busesWithDrivers = await _context.GetAllBusesWithDriverName();
            return Ok(busesWithDrivers);      
        }
        // POST: api/Buses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("CreateBusByAdmin")]
        public async Task<ActionResult<Bus>> PostBus(BusDTO busDTO)
        {
            return await _context.CreateBusInfo(busDTO);
        }
        // Get
        [HttpGet("AllBussesLocations")]
        public async Task<ActionResult<List<StopPointWithCurrentLocation>>> AllBussesLosations()
        {
            return await _context.GetAllBusesLocations();
        }
        // Get the current location and stop points by driver ID
        [HttpGet("CurrentLocationByDriverId")]
        public async Task<ActionResult<StopPointWithCurrentLocation>> GetCurrentLocationByDriverId(string driverId)
        {
            var result = await _context.CurrentLocationByDriverId(driverId);
            return Ok(result);
        }
        // PUT: api/Buses/UpdateCurrentLocation
        [HttpPut("UpdateCurrentLocation")]
        public async Task<ActionResult<Bus>> UpdateCurrentLocation(string driverId, string currentLocation)
        {
            var updatedBus = await _context.UpdateCurrentLocationByDriverId(driverId, currentLocation);
            return Ok(updatedBus);
        }
    }
}