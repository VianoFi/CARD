using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CARD.Data;
using CARD.Models;
using System.Threading.Tasks;

namespace CARD.Controllers
{
    public class PokemonCardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PokemonCardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PokemonCard
        public async Task<IActionResult> Index()
        {
            var cards = await _context.PokemonCards.ToListAsync();
            return View(cards);
        }

        // GET: PokemonCard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var card = await _context.PokemonCards.FirstOrDefaultAsync(c => c.Id == id);
            if (card == null) return NotFound();

            return View(card);
        }

        // GET: PokemonCard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PokemonCard/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PokemonCard card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: PokemonCard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var card = await _context.PokemonCards.FindAsync(id);
            if (card == null) return NotFound();

            return View(card);
        }

        // POST: PokemonCard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PokemonCard card)
        {
            if (id != card.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PokemonCards.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: PokemonCard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var card = await _context.PokemonCards.FirstOrDefaultAsync(m => m.Id == id);
            if (card == null) return NotFound();

            return View(card);
        }

        // POST: PokemonCard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.PokemonCards.FindAsync(id);
            if (card != null)
            {
                _context.PokemonCards.Remove(card);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
