using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebInterface.Data;

namespace WebInterface.Controllers
{
    public class BanksController : Controller
    {
        private readonly AppDbContext context;

        public BanksController(AppDbContext context)
        {
            this.context = context;
        }

        //Get list of banks.
        public async Task<IActionResult> Index()
        {
            var data = await context.Banks.ToListAsync();
            return View(data);
        }
    }
}
