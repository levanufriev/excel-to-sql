using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebInterface.Data;

namespace WebInterface.Controllers
{
    public class SheetsController : Controller
    {
        private readonly AppDbContext context;

        public SheetsController(AppDbContext context)
        {
            this.context = context;
        }

        //Get list of rows from Sheet table for the current bank.
        public async Task<IActionResult> Index(int id)
        {
            var data = await context.Sheets.Where(s => s.BankId == id).ToListAsync();
            return View(data);
        }
    }
}
