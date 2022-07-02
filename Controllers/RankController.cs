using Document_Request.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public RankController(DatabaseContext context)
        {
            _context = context;
        }

        //GET all ranks
        [HttpGet]
        public async Task<IActionResult> GetRanks()
        {
            var ranks = await _context.Ranks.ToListAsync();
            if (ranks == null)
            {
                return NotFound("No rank were found");
            }

            return Ok(ranks);
        }

        //GET rank
        [HttpGet("{id:guid}")]
        [ActionName("GetRank")]
        public async Task<IActionResult> GetRank([FromRoute] Guid id)
        {
            var rank = await _context.Ranks.FirstOrDefaultAsync(t => t.Id == id);
            if (rank == null)
            {
                return NotFound("Rank not found");
            }

            return Ok(rank);

        }

        //POST rank
        [HttpPost]
        public async Task<IActionResult> AddRank([FromBody] Rank rank)
        {
            rank.Id = Guid.NewGuid();
            await _context.Ranks.AddAsync(rank);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRank), new { id = rank.Id }, rank);
        }

        //PUT rank
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRank([FromRoute] Guid id, [FromBody] Rank newRank)
        {
            var oldRank = await _context.Ranks.FindAsync(id);
            if (oldRank == null)
            {
                return NotFound("Rank not found!");
            }

            oldRank.Name = newRank.Name;
            await _context.SaveChangesAsync();

            return Ok(oldRank);
        }

        //DELETE rank
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRank([FromRoute] Guid id)
        {
            var rank = await _context.Ranks.FindAsync(id);
            if (rank == null)
            {
                return NotFound("Rank not found!");
            }

            _context.Remove(rank);
            await _context.SaveChangesAsync();

            return Ok(rank);
        }
    }
}
