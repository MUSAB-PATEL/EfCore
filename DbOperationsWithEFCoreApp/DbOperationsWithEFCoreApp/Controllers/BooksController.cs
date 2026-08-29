using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpPost("AddBook")]
        public async Task<IActionResult> AddNewBook([FromBody] Book model)
        {
            if (model == null)
            {
                return BadRequest("Book model is null.");
            }
            // Set the CreatedOn property to the current date and time
            model.CreatedOn = DateTime.UtcNow;
            // Add the new book to the database
            await appDbContext.Books.AddAsync(model);
            await appDbContext.SaveChangesAsync();
            return Ok(new { message = "Book added successfully.", bookId = model.Id });
        }

    }
}
