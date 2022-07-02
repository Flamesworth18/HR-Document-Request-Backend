using Document_Request.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmountController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AmountController(DatabaseContext context)
        {
            _context = context;
        }

        //GET all amounts
        [HttpGet]
        public async Task<IActionResult> GetAmounts()
        {
            var amount = await _context.Amounts.ToListAsync();
            if (amount == null)
            {
                return NotFound("No amounts were found");
            }

            return Ok(amount);
        }

        //GET amount
        [HttpGet("{id:guid}")]
        [ActionName("GetAmount")]
        public async Task<IActionResult> GetAmount([FromRoute] Guid id)
        {
            var amount = await _context.Amounts.FirstOrDefaultAsync(t => t.Id == id);
            if (amount == null)
            {
                return NotFound("Amount not found");
            }

            return Ok(amount);

        }

        //POST amount
        [HttpPost]
        public async Task<IActionResult> AddAmount([FromBody] Amount amount)
        {
            amount.Id = Guid.NewGuid();
            await _context.Amounts.AddAsync(amount);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAmount), new { id = amount.Id }, amount);
        }

        //PUT amount
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAmount([FromRoute] Guid id, [FromBody] Amount newAmount)
        {
            var oldAmount = await _context.Amounts.FindAsync(id);
            if (oldAmount == null)
            {
                return NotFound("Amount not found!");
            }

            oldAmount.Number = newAmount.Number;
            await _context.SaveChangesAsync();

            return Ok(oldAmount);
        }

        //DELETE amount
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAmount([FromRoute] Guid id)
        {
            var amount = await _context.Amounts.FindAsync(id);
            if (amount == null)
            {
                return NotFound("Amount not found!");
            }

            _context.Amounts.Remove(amount);
            await _context.SaveChangesAsync();

            return Ok(amount);
        }
    }
}
