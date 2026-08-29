using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController(AppDbContext appDbContext) : ControllerBase
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        [HttpGet("GetAllCurrencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            //var result = _appDbContext.Currencies.ToList();
            var result = await (from currencies in _appDbContext.Currencies
                                select currencies).ToListAsync();
            return Ok(result);
        }

        [HttpGet("GetCurrencyById")]
        public async Task<IActionResult> GetCurrencyById(int id)
        {
            var result = await _appDbContext.Currencies.FindAsync(id);
            return Ok(result);
        }

        [HttpGet("GetCurrencyByTitle")]
        public async Task<IActionResult> GetCurrencyByTitle(string title, string description)
        {
            //var result = await _appDbContext.Currencies.FirstOrDefaultAsync(c => c.Title == title && c.Description == description );
            var result = await _appDbContext.Currencies.Where(c => c.Title == title && c.Description == description).ToListAsync();

            return Ok(result);
        }

        [HttpPost("GetMultipleCurrenciesByIds")]
        public async Task<IActionResult> GetMultipleCurrenciesByIds([FromBody] List<int> ids)
        {
            var result = await _appDbContext.Currencies.
                Where(c => ids.Contains(c.Id))
                //.Select(c => new { c.Id, c.Title, c.Description }) //Anonymous type
                .Select(c => new Currency()
                {
                    Id = c.Id,
                    Title = c.Title
                })
                .ToListAsync();
            return Ok(result);
        }
    }
}
