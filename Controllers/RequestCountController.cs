using Document_Request.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestCountController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public RequestCountController(DatabaseContext context)
        {
            _context = context;
        }

        //GET total request
        [HttpGet]
        public async Task<IActionResult> GetRequestsCount()
        {
            var totalRequest = await _context.Requests.ToListAsync();
            if(totalRequest == null)
            {
                return NotFound("Requests are not found!");
            }

            var requestCounts = await _context.RequestCounts.ToListAsync();
            if(requestCounts == null)
            {
                return NotFound("Requests count not found!");
            }

            var date = DateTime.UtcNow.ToShortDateString();
            var todayRequest = totalRequest.FindAll(r => r.DateCreated == date);
            if(todayRequest == null)
            {
                requestCounts[0].TodayCount = 0;
            }
            else
            {
                requestCounts[0].TodayCount = todayRequest.Count;
            }

            requestCounts[0].TotalCount = totalRequest.Count;

            return Ok(requestCounts[0]);
        }

        //GET total requests
        [HttpGet("{id:guid}")]
        [ActionName("GetRequestCounts")]
        public async Task<IActionResult> GetRequestCounts([FromRoute] Guid id)
        {
            var total = await _context.RequestCounts.FirstOrDefaultAsync(t => t.Id == id);
            if (total == null)
            {
                return BadRequest(total);
            }

            return Ok(total);
        }

        //POST total
        [HttpPost]
        public async Task<IActionResult> AddTotal([FromBody] RequestCount total)
        {
            total.Id = Guid.NewGuid();
            await _context.RequestCounts.AddAsync(total);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRequestCounts), new { id = total.Id }, total);
        }
    }
}
