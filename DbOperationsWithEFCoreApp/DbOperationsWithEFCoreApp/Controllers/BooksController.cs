using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpPost("AddNewBook")]
        public async Task<IActionResult> AddNewBook([FromBody] Book model)
        {
            if (model == null)
            {
                return BadRequest("Book modelv  is null.");
            }
            // Set the CreatedOn property to the current date and time
            model.CreatedOn = DateTime.UtcNow;
            // Add the new book to the database
            await appDbContext.Books.AddAsync(model);
            await appDbContext.SaveChangesAsync();
            return Ok(new { message = "Book added successfully.", bookId = model.Id });
        }

        [HttpPost("AddMultipleBooks")]
        public async Task<IActionResult> AddMultipleBooks([FromBody] List<Book> books)
        {
            if (books == null || !books.Any())
            {
                return BadRequest("Book list is null or empty.");
            }
            // Set the CreatedOn property for each book to the current date and time
            foreach (var book in books)
            {
                book.CreatedOn = DateTime.UtcNow;
            }
            // Add the new books to the database
            await appDbContext.Books.AddRangeAsync(books);
            await appDbContext.SaveChangesAsync();
            return Ok(new { message = "Books added successfully.", bookCount = books.Count });
        }

        [HttpPut("UpdateBook")]
        public async Task<IActionResult> UpdateBook([FromBody] Book model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest("Invalid book model.");
            }
            var existingBook = await appDbContext.Books.FindAsync(model.Id);
            if (existingBook == null)
            {
                return NotFound($"Book with ID {model.Id} not found."); 
            }
            // Update the properties of the existing book
            existingBook.Title = model.Title;
            existingBook.Description = model.Description;
            existingBook.LanguageId = model.LanguageId;
            existingBook.NoOfPages = model.NoOfPages;
            existingBook.IsActive = model.IsActive;
            existingBook.CreatedOn = DateTime.UtcNow;
            await appDbContext.SaveChangesAsync();
            return Ok(new { message = "Book updated successfully.", bookId = existingBook.Id });
        }

        [HttpPut("UpdateWithSingleQuery")]
        public async Task<IActionResult> UpdateWithSingleQuery([FromBody] Book model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest("Invalid book model.");
            }
            // Using this approach, we can update the book in a single query without fetching it first.
            // But we need to ensure that the model contains all the necessary/required properties, including the Id.
            // If any property is missing which is not reqiured, it will be set to its default value in the database or it will be set as NULL.
            // Otherwise, if the property is required and not provided, it will throw an exception.
            appDbContext.Books.Update(model);
            await appDbContext.SaveChangesAsync();

            return Ok(new { message = "Book updated successfully with a single query.", bookId = model.Id });
        }

        [HttpPut("BulkUpdateBooks")]
        public async Task<IActionResult> BulkUpdateBooks()
        {
            await appDbContext.Books
                .Where(p => p.NoOfPages == 10)   //Use where for where condition
                .ExecuteUpdateAsync(x => x
            .SetProperty(p => p.Title, p => p.Title.Replace("updated", ""))
            .SetProperty(p => p.Description, p => p.Description + "updated")
            );
            // Here we have'nt used and SaveChangesAsync method
            // because it is only used when we are updating the entity after fetching it from the database.
            // But in this case, we are directly updating the entity in the database without fetching it first.
            // So, we don't need to call SaveChangesAsync method.
            // SaveChangesAsync only works on basis of change tracker.
            return Ok(new { message = "Books updated successfully in bulk." });
        }
    }
}
 