using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CARD.Models;
using CARD.Data;
using Microsoft.EntityFrameworkCore;

namespace CARD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            var totalPokemon = await _context.PokemonCards.SumAsync(c => c.Valore);
            var totalYuGiOh = await _context.YuGiOhCards.SumAsync(c => c.Valore);
            var totalMagic = await _context.MagicCards.SumAsync(c => c.Valore);

            ViewBag.TotalPokemon = totalPokemon;
            ViewBag.TotalYuGiOh = totalYuGiOh;
            ViewBag.TotalMagic = totalMagic;
            ViewBag.TotalGenerale = totalPokemon + totalYuGiOh + totalMagic;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
