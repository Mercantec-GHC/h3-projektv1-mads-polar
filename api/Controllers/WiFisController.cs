using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WiFisController : ControllerBase
    {
        private readonly AppDBContext _context;

        public WiFisController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/WiFis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WiFi>>> GetWiFi()
        {
            return await _context.WiFi.ToListAsync();
        }

        // GET: api/WiFis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WiFi>> GetWiFi(string id)
        {
            var wiFi = await _context.WiFi.FindAsync(id);

            if (wiFi == null)
            {
                return NotFound();
            }

            return wiFi;
        }

        // PUT: api/WiFis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWiFi(string id, WiFi wiFi)
        {
            if (id != wiFi.Id)
            {
                return BadRequest();
            }

            _context.Entry(wiFi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WiFiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WiFis
        [HttpPost]
        public async Task<ActionResult> SetWiFiCredentials(WiFiDTO wiFiDTO)
        {
            if (string.IsNullOrEmpty(wiFiDTO.DeviceId))
            {
                return BadRequest("Device ID is required.");
            }

            // Check if the device exists
            bool deviceExists = await _context.DeviceData.AnyAsync(d => d.DeviceId == wiFiDTO.DeviceId);

            if (!deviceExists)
            {
                return NotFound("Device ID does not exist in the database.");
            }

            // Map WiFiDTO to WiFi entity
            var addWiFiCredentials = MapWiFiDTOToDeviceData(wiFiDTO);

            // Add WiFi credentials to the database
            _context.WiFi.Add(addWiFiCredentials);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving data to the database.");
            }

            return Ok("WiFi credentials added successfully.");
        }

        // DELETE: api/WiFis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWiFi(string id)
        {
            var wiFi = await _context.WiFi.FindAsync(id);
            if (wiFi == null)
            {
                return NotFound();
            }

            _context.WiFi.Remove(wiFi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WiFiExists(string id)
        {
            return _context.WiFi.Any(e => e.Id == id);
        }

        private WiFi MapWiFiDTOToDeviceData(WiFiDTO wiFiDTO)
        {
            return new WiFi
            {
                Id = Guid.NewGuid().ToString("N"),
                WiFiName = wiFiDTO.WiFiName,
                WiFiPassword = wiFiDTO.WiFiPassword,
                DeviceId = wiFiDTO.DeviceId,
                CreatedAt = DateTime.UtcNow.AddHours(2),
                UpdatedAt = DateTime.UtcNow.AddHours(2),
            };
        }
    }
}
