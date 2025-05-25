using Microsoft.AspNetCore.Mvc;
using CARD.Data;
using CARD.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CARD.Controllers
{
    public class MagicCardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MagicCardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MagicCard
        public async Task<IActionResult> Index()
        {
            var cards = await _context.MagicCards.ToListAsync();
            return View(cards);
        }

        // GET: MagicCard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var card = await _context.MagicCards.FirstOrDefaultAsync(m => m.Id == id);
            if (card == null) return NotFound();

            return View(card);
        }

        // GET: MagicCard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MagicCard/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MagicCard card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: MagicCard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var card = await _context.MagicCards.FindAsync(id);
            if (card == null) return NotFound();

            return View(card);
        }

        // POST: MagicCard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MagicCard card)
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
                    if (!MagicCardExists(card.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: MagicCard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var card = await _context.MagicCards.FirstOrDefaultAsync(m => m.Id == id);
            if (card == null) return NotFound();

            return View(card);
        }

        // POST: MagicCard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.MagicCards.FindAsync(id);
            if (card != null)
            {
                _context.MagicCards.Remove(card);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MagicCardExists(int id)
        {
            return _context.MagicCards.Any(e => e.Id == id);
        }
    }
}
