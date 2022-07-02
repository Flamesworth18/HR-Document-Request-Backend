using Document_Request.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public DirectorController(DatabaseContext context)
        {
            _context = context;
        }

        //GET all directors
        [HttpGet]
        public async Task<IActionResult> GetDirectors()
        {
            var dir = await _context.Directors.ToListAsync();
            if (dir == null)
            {
                return NotFound("No directors were found");
            }

            return Ok(dir);
        }

        //GET director
        [HttpGet("{id:guid}")]
        [ActionName("GetDirector")]
        public async Task<IActionResult> GetDirector([FromRoute] Guid id)
        {
            var dir = await _context.Directors.FirstOrDefaultAsync(t => t.Id == id);
            if (dir == null)
            {
                return NotFound("Director not found");
            }

            return Ok(dir);

        }

        //POST director
        [HttpPost]
        public async Task<IActionResult> AddDirector([FromBody] Director dir)
        {
            dir.Id = Guid.NewGuid();
            await _context.Directors.AddAsync(dir);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDirector), new { id = dir.Id }, dir);
        }

        //PUT director
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDirector([FromRoute] Guid id, [FromBody] Director newDir)
        {
            var oldDir = await _context.Directors.FindAsync(id);
            if (oldDir == null)
            {
                return NotFound("Director not found!");
            }

            oldDir.Name = newDir.Name;
            await _context.SaveChangesAsync();

            return Ok(oldDir);
        }

        //DELETE director
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDirector([FromRoute] Guid id)
        {
            var dir = await _context.Directors.FindAsync(id);
            if (dir == null)
            {
                return NotFound("Director not found!");
            }

            _context.Directors.Remove(dir);
            await _context.SaveChangesAsync();

            return Ok(dir);
        }
    }
}
