using Document_Request.Data;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public RequestsController(DatabaseContext context)
        {
            _context = context;
        }

        //GET all requests
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _context.Requests.ToListAsync();
            if (requests == null)
            {
                return NotFound("No requests were found");
            }

            return Ok(requests);
        }

        //GET request
        [HttpGet("{id:guid}")]
        [ActionName("GetRequest")]
        public async Task<IActionResult> GetRequest([FromRoute] Guid id)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(t => t.Id == id);
            if (request == null)
            {
                return NotFound("Request not found");
            }

            return Ok(request);

        }

        //POST request
        [HttpPost]
        public async Task<IActionResult> AddRequest([FromBody] Request request)
        {
            request.Id = Guid.NewGuid();
            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
        }

        //PUT request
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRequest([FromRoute] Guid id, [FromBody] Request newRequest)
        {
            var oldRequest = await _context.Requests.FindAsync(id);
            if (oldRequest == null)
            {
                return NotFound("Request not found!");
            }

            oldRequest.FirstName = newRequest.FirstName;
            oldRequest.LastName = newRequest.LastName;
            oldRequest.Purpose = newRequest.Purpose;
            oldRequest.Sex = newRequest.Sex;
            oldRequest.Rank = newRequest.Rank;
            oldRequest.Document = newRequest.Document;
            oldRequest.Status = newRequest.Status;
            await _context.SaveChangesAsync();

            return Ok(oldRequest);
        }

        //DELETE request
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRequest([FromRoute] Guid id)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
            {
                return NotFound("Request not found!");
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return Ok(request);
        }
    }
}
