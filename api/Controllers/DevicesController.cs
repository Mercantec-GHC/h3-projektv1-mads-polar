using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public DevicesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetDevice()
        {
            return await _context.Device.ToListAsync();
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Device>> GetDevice(string id)
        {
            var device = await _context.Device.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return device;
        }

        // PUT: api/Devices/
        [HttpPut]
        public async Task<IActionResult> PutDevice(string id, PutDeviceDTO putDeviceDTO) // Class instance of the class
        {
            // Find the device in the database
            var device = await _context.Device.FindAsync(id);
            device.DeviceStatus = putDeviceDTO.DeviceStatus;
            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

        // POST: api/Devices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(CreateDeviceDTO createDeviceDTO)
        {

            var createDevice = MapcreateDeviceDTOToDevice(createDeviceDTO);

            _context.Device.Add(createDevice);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }

            return Ok(new { createDevice.Id });
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(string id)
        {
            var device = await _context.Device.FindAsync(id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Device.Remove(device);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceExists(string id)
        {
            return _context.Device.Any(e => e.Id == id);
        }

        private Device MapcreateDeviceDTOToDevice(CreateDeviceDTO createDeviceDTO)
        {

            return new Device
            {
                Id = Guid.NewGuid().ToString("N"),
                DeviceStatus = Status.Disarmed,
                DeviceLocation = createDeviceDTO.DeviceLocation,
                CreatedAt = DateTime.UtcNow.AddHours(2),
                UpdatedAt = DateTime.UtcNow.AddHours(2),
            };
        }
    }
}
