using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CARD.Models;
using CARD.Data;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace CARD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _config = config;

            // Inizializza la chiave segreta di Stripe
            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }

        public IActionResult Index()
        {
            ViewBag.StripePublishableKey = _config["Stripe:PublishableKey"];
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

        // ================== Stripe Donation ==================
        [HttpPost]
        public IActionResult Donate(decimal amount)
        {
            // Convertiamo in centesimi perché Stripe lavora con unit_amount intero
            long amountInCents = (long)(amount * 100);

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
        {
            new Stripe.Checkout.SessionLineItemOptions
            {
                PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                {
                    UnitAmount = amountInCents,
                    Currency = "eur",
                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Donazione per CARD"
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = Url.Action("Success", "Home", null, Request.Scheme, Request.Host.ToString()),
                CancelUrl = Url.Action("Cancel", "Home", null, Request.Scheme, Request.Host.ToString())
            };

            var service = new Stripe.Checkout.SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }


        public IActionResult Success()
        {
            TempData["SuccessMessage"] = "Grazie per la tua donazione!";
            return RedirectToAction("Index");
        }

        public IActionResult Cancel()
        {
            TempData["ErrorMessage"] = "Hai annullato la donazione.";
            return RedirectToAction("Index");
        }
    }
}
