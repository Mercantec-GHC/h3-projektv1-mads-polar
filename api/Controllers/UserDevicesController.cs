using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDevicesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public UserDevicesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/UserDevices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDevice>>> GetUserDevice()
        {
            return await _context.UserDevice.ToListAsync();
        }

        // GET: api/UserDevices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDevice>> GetUserDevice(string id)
        {
            var userDevice = await _context.UserDevice.FindAsync(id);

            if (userDevice == null)
            {
                return NotFound();
            }

            return userDevice;
        }

        // PUT: api/UserDevices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDevice(string id, UserDevice userDevice)
        {
            if (id != userDevice.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDevice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDeviceExists(id))
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

        // POST: api/UserDevices
        [HttpPost]
        public async Task<ActionResult<UserDevice>> PostUserDevice(UserDeviceDTO userDeviceDTO)
        {
            if (string.IsNullOrEmpty(userDeviceDTO.DeviceId) || string.IsNullOrEmpty(userDeviceDTO.UserId))
            {
                return BadRequest("Device ID or User ID is required.");
            }

            bool deviceExists = await _context.Device.AnyAsync(d => d.Id == userDeviceDTO.DeviceId);

            bool userExists = await _context.User.AnyAsync(u => u.Id == userDeviceDTO.UserId);

            if (!deviceExists || !userExists)
            {
                return NotFound("Device or User does not exist in database.");
            }

            var addUserDevice = MapuserDeviceDTOToUserDevice(userDeviceDTO);

            _context.UserDevice.Add(addUserDevice);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }

            return Ok("User Device added succesfully");
        }

        // DELETE: api/UserDevices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDevice(string id)
        {
            var userDevice = await _context.UserDevice.FindAsync(id);
            if (userDevice == null)
            {
                return NotFound();
            }

            _context.UserDevice.Remove(userDevice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserDeviceExists(string id)
        {
            return _context.UserDevice.Any(e => e.Id == id);
        }

        private UserDevice MapuserDeviceDTOToUserDevice(UserDeviceDTO userDeviceDTO)
        {

            return new UserDevice
            {
                Id = Guid.NewGuid().ToString("N"),
                UserId = userDeviceDTO.UserId,
                DeviceId = userDeviceDTO.DeviceId,
                CreatedAt = DateTime.UtcNow.AddHours(2),
                UpdatedAt = DateTime.UtcNow.AddHours(2),
            };
        }
    }
}
