using Microsoft.AspNetCore.Mvc;
using CARD.Models;

namespace CARD.Controllers
{
    public class DemoController : Controller
    {
        [HttpGet]
        public IActionResult HelperTag()
        {
            return View(new DemoHelperTagsViewModel());
        }

        [HttpPost]
        public IActionResult HelperTag(DemoHelperTagsViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = $"Dati ricevuti: {model.Nome}, {model.Email}, Paese: {model.Paese}";
            }

            return View(model);
        }
    }
}