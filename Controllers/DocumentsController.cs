using Document_Request.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_Request.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public DocumentsController(DatabaseContext context)
        {
            _context = context;
        }

        //GET all documents
        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var doc = await _context.Documents.ToListAsync();
            if(doc == null)
            {
                return NotFound("No documents were found");
            }

            return Ok(doc);
        }

        //GET document
        [HttpGet("{id:guid}")]
        [ActionName("GetDocument")]
        public async Task<IActionResult> GetDocument([FromRoute] Guid id)
        {
            var doc = await _context.Documents.FirstOrDefaultAsync(t => t.Id == id);
            if(doc == null)
            {
                return NotFound("Document not found");
            }

            return Ok(doc);

        }

        //POST document
        [HttpPost]
        public async Task<IActionResult> AddDocument([FromBody] DocumentType doc)
        {
            doc.Id = Guid.NewGuid();
            await _context.Documents.AddAsync(doc);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocument), new { id = doc.Id }, doc);
        }

        //PUT document
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateDocument([FromRoute] Guid id, [FromBody] DocumentType doc)
        {
            var oldDoc = await _context.Documents.FindAsync(id);
            if(oldDoc == null)
            {
                return NotFound("Document not found!");
            }

            oldDoc.Name = doc.Name;
            await _context.SaveChangesAsync();

            return Ok(oldDoc);
        }

        //DELETE document
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDocument([FromRoute] Guid id)
        {
            var doc = await _context.Documents.FindAsync(id);
            if(doc == null)
            {
                return NotFound("Document not found!");
            }

            _context.Remove(doc);
            await _context.SaveChangesAsync();

            return Ok(doc);
        }
    }
}
