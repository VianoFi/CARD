using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CARD.Data;
using CARD.Models;

namespace CARD.Controllers
{
    public class YuGiOhCardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public YuGiOhCardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: YuGiOhCard
        public async Task<IActionResult> Index()
        {
            var cards = await _context.YuGiOhCards.ToListAsync();
            return View(cards);
        }

        // GET: YuGiOhCard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var card = await _context.YuGiOhCards.FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
                return NotFound();

            return View(card);
        }

        // GET: YuGiOhCard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: YuGiOhCard/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(YuGiOhCard card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: YuGiOhCard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var card = await _context.YuGiOhCards.FindAsync(id);
            if (card == null)
                return NotFound();

            return View(card);
        }

        // POST: YuGiOhCard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, YuGiOhCard card)
        {
            if (id != card.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YuGiOhCardExists(card.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(card);
        }

        // GET: YuGiOhCard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var card = await _context.YuGiOhCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
                return NotFound();

            return View(card);
        }

        // POST: YuGiOhCard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.YuGiOhCards.FindAsync(id);
            if (card != null)
            {
                _context.YuGiOhCards.Remove(card);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool YuGiOhCardExists(int id)
        {
            return _context.YuGiOhCards.Any(e => e.Id == id);
        }
    }
}
