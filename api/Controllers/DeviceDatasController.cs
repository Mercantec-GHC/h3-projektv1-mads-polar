using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceDatasController : ControllerBase
    {
        private readonly AppDBContext _context;

        public DeviceDatasController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/DeviceDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceData>>> GetDeviceData()
        {
            return await _context.DeviceData.ToListAsync();
        }

        // GET: api/DeviceDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceData>> GetDeviceData(string id)
        {
            var deviceData = await _context.DeviceData.FindAsync(id);

            if (deviceData == null)
            {
                return NotFound();
            }

            return deviceData;
        }

        // PUT: api/DeviceDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceData(string id, DeviceData deviceData)
        {
            if (id != deviceData.Id)
            {
                return BadRequest();
            }

            _context.Entry(deviceData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceDataExists(id))
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

        // POST: api/DeviceDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeviceData>> PostDeviceData(DeviceDataDTO deviceDataDTO)
        {
            var addDeviceData = MapDeviceDataDTOToDeviceData(deviceDataDTO);

            _context.DeviceData.Add(addDeviceData);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }

            return Ok("Device data added succesfully");
        }

        // DELETE: api/DeviceDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceData(string id)
        {
            var deviceData = await _context.DeviceData.FindAsync(id);
            if (deviceData == null)
            {
                return NotFound();
            }

            _context.DeviceData.Remove(deviceData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceDataExists(string id)
        {
            return _context.DeviceData.Any(e => e.Id == id);
        }

        private DeviceData MapDeviceDataDTOToDeviceData(DeviceDataDTO deviceDataDTO)
        {

            return new DeviceData
            {
                Id = Guid.NewGuid().ToString("N"),
                BatteryLevel = deviceDataDTO.BatteryLevel,
                CreatedAt = DateTime.UtcNow.AddHours(2),
                UpdatedAt = DateTime.UtcNow.AddHours(2),
            };
        }
    }
}
