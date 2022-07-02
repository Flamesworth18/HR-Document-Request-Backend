using Document_Request.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurposeController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public PurposeController(DatabaseContext context)
        {
            _context = context;
        }

        //GET all purposes
        [HttpGet]
        public async Task<IActionResult> GetPurposes()
        {
            var purposes = await _context.Purposes.ToListAsync();
            if (purposes == null)
            {
                return NotFound("No purposes were found");
            }

            return Ok(purposes);
        }

        //GET document
        [HttpGet("{id:guid}")]
        [ActionName("GetPurpose")]
        public async Task<IActionResult> GetPurpose([FromRoute] Guid id)
        {
            var purpose = await _context.Purposes.FirstOrDefaultAsync(t => t.Id == id);
            if (purpose == null)
            {
                return NotFound("Purpose not found");
            }

            return Ok(purpose);

        }

        //POST purpose
        [HttpPost]
        public async Task<IActionResult> AddPurpose([FromBody] Purpose purpose)
        {
            purpose.Id = Guid.NewGuid();
            await _context.Purposes.AddAsync(purpose);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPurpose), new { id = purpose.Id }, purpose);
        }

        //PUT purpose
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePurpose([FromRoute] Guid id, [FromBody] Purpose newPurpose)
        {
            var oldPurpose = await _context.Purposes.FindAsync(id);
            if (oldPurpose == null)
            {
                return NotFound("Purpose not found!");
            }

            oldPurpose.Name = newPurpose.Name;
            await _context.SaveChangesAsync();

            return Ok(oldPurpose);
        }

        //DELETE purpose
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePurpose([FromRoute] Guid id)
        {
            var purpose = await _context.Purposes.FindAsync(id);
            if (purpose == null)
            {
                return NotFound("Purpose not found!");
            }

            _context.Remove(purpose);
            await _context.SaveChangesAsync();

            return Ok(purpose);
        }
    }
}
