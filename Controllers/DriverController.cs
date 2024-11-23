using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MASAR.Data;
using MASAR.Model;
using Microsoft.AspNetCore.Authorization;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using System.IO;

namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriver _context;
        public DriverController(IDriver context)
        {
            _context = context;
        }
        //GET : api/Driver
        [HttpGet("ViewAllAnnouncement")]
        [Authorize(Policy = "RequireDriverRole")]
        public async Task<List<AnnouncementDTO>> ViewAllAnnouncement()
        {
            return await _context.ViewAnnouncements();
        }
        //POST : api/Driver
        [HttpPost("CreateDriverInfo")]
        [Authorize (Policy = "RequireDriverRole")]
        public async Task<DriverProfile> CreateInfo(string email, DriverInfoDTO driveInfo)
        {
            return await _context.CreateInfo(email, driveInfo);
        }
        //POST : api/Driver
        [HttpPost("UpdateDriverProfile")]
        [Authorize(Policy = "RequireDriverRole")]
        public async Task<DriverInfoDTO> UpdateDriver(string email, DriverInfoDTO driver)
        {
            return await _context.UpdateDriver(email, driver);
        }
        //POST : api/Driver
        [HttpPost("MaintenanceRequest")]
        [Authorize(Policy = "RequireDriverRole")]
        public async Task<Maintenance> MaintenanceRequest(string email, MaintenancaDTO maintenanceDTO)
        {
            return await _context.MaintenanceRequest(email, maintenanceDTO);
        }
    }
}