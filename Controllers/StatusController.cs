using Document_Request.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public StatusController(DatabaseContext context)
        {
            _context = context;
        }

        //GET all statuses
        [HttpGet]
        public async Task<IActionResult> GetStatuses()
        {
            var statuses = await _context.Statuses.ToListAsync();
            if (statuses == null)
            {
                return NotFound("No statuses were found");
            }

            return Ok(statuses);
        }

        //GET status
        [HttpGet("{id:guid}")]
        [ActionName("GetStatus")]
        public async Task<IActionResult> GetStatus([FromRoute] Guid id)
        {
            var status = await _context.Statuses.FirstOrDefaultAsync(t => t.Id == id);
            if (status == null)
            {
                return NotFound("Status not found");
            }

            return Ok(status);

        }

        //POST status
        [HttpPost]
        public async Task<IActionResult> AddStatus([FromBody] Status status)
        {
            status.Id = Guid.NewGuid();
            await _context.Statuses.AddAsync(status);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStatus), new { id = status.Id }, status);
        }

        //PUT status
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] Status newStatus)
        {
            var oldStatus = await _context.Statuses.FindAsync(id);
            if (oldStatus == null)
            {
                return NotFound("Status not found!");
            }

            oldStatus.Name = newStatus.Name;
            await _context.SaveChangesAsync();

            return Ok(oldStatus);
        }

        //DELETE status
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStatus([FromRoute] Guid id)
        {
            var status = await _context.Statuses.FindAsync(id);
            if (status == null)
            {
                return NotFound("Status not found!");
            }

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();

            return Ok(status);
        }
    }
}
